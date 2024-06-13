/*
    This script is a wrapper for the svelte json editor (standalone version).
    With specific elements and functions, the editor can be interacted with.
	
	- div element with id 'jse-container'
	    Represents the container for the editor (see also function 'initialize')
	
	- button element with id 'jse-interface'
	    The 'change' event is triggered by this script with specific ChangeEventArgs:
	    "0": the content has been changed by the editor
	    "1": the open button has been clicked
	    "2" + json content: the save button has been clicked
	    "3" + json content: the saveAs button has been clicked
	    
	- function 'initialize'
	    Initializes the editor and adds it to the 'jse-container' div.

    - function 'setContent'
        Sets the content for the editor when invoked with the specific content as string.

    - function 'disableCanSave'
        Disables the save button when invoked.
*/

import { JSONEditor } from './standalone.js'

let editor = undefined
let canBeSaved = false

window.setContent = (content) => {
    if (editor === undefined) return
    editor.set({
        text: content,
        json: undefined
    })
}

window.disableCanSave = () => {
    if (editor === undefined) return
    canBeSaved = false
    
    // update the readOnly property. this triggers the menu to re-render
    // readOnly will be reset when handling onRenderMenu
    editor.updateProps({
        readOnly: true
    })
}

window.initialize = () => {
    
    if (editor !== undefined) return

    const div = document.getElementById("jse-container")
    const button = document.getElementById("jse-interface")
    const event = new Event("change");
    
    // Custom 'Open' button
    const openButton = {
        type: "button",
        title: "Open file",
        icon: {prefix: '', iconName: '', icon: [16, 16, [], "", "M1 3.5A1.5 1.5 0 0 1 2.5 2h2.764c.958 0 1.76.56 2.311 1.184C7.985 3.648 8.48 4 9 4h4.5A1.5 1.5 0 0 1 15 5.5v.64c.57.265.94.876.856 1.546l-.64 5.124A2.5 2.5 0 0 1 12.733 15H3.266a2.5 2.5 0 0 1-2.481-2.19l-.64-5.124A1.5 1.5 0 0 1 1 6.14zM2 6h12v-.5a.5.5 0 0 0-.5-.5H9c-.964 0-1.71-.629-2.174-1.154C6.374 3.334 5.82 3 5.264 3H2.5a.5.5 0 0 0-.5.5zm-.367 1a.5.5 0 0 0-.496.562l.64 5.124A1.5 1.5 0 0 0 3.266 14h9.468a1.5 1.5 0 0 0 1.489-1.314l.64-5.124A.5.5 0 0 0 14.367 7z"]},
        onClick: HandleOnOpen
    }

    // Custom 'Save' button
    const saveButton = {
        type: "button",
        title: "Save",
        icon: {prefix: '', iconName: '', icon: [16, 16, [], "", "M11 2H9v3h2z M1.5 0h11.586a1.5 1.5 0 0 1 1.06.44l1.415 1.414A1.5 1.5 0 0 1 16 2.914V14.5a1.5 1.5 0 0 1-1.5 1.5h-13A1.5 1.5 0 0 1 0 14.5v-13A1.5 1.5 0 0 1 1.5 0M1 1.5v13a.5.5 0 0 0 .5.5H2v-4.5A1.5 1.5 0 0 1 3.5 9h9a1.5 1.5 0 0 1 1.5 1.5V15h.5a.5.5 0 0 0 .5-.5V2.914a.5.5 0 0 0-.146-.353l-1.415-1.415A.5.5 0 0 0 13.086 1H13v4.5A1.5 1.5 0 0 1 11.5 7h-7A1.5 1.5 0 0 1 3 5.5V1H1.5a.5.5 0 0 0-.5.5m3 4a.5.5 0 0 0 .5.5h7a.5.5 0 0 0 .5-.5V1H4zM3 15h10v-4.5a.5.5 0 0 0-.5-.5h-9a.5.5 0 0 0-.5.5z"]},
        onClick: HandleOnSave
    }

    // Custom 'SaveAs' button
    const saveAsButton = {
        type: "button",
        title: "Save as",
        icon: {prefix: '', iconName: '', icon: [16, 16, [], "", "M2 1a1 1 0 0 0-1 1v12a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H9.5a1 1 0 0 0-1 1v7.293l2.646-2.647a.5.5 0 0 1 .708.708l-3.5 3.5a.5.5 0 0 1-.708 0l-3.5-3.5a.5.5 0 1 1 .708-.708L7.5 9.293V2a2 2 0 0 1 2-2H14a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2V2a2 2 0 0 1 2-2h2.5a.5.5 0 0 1 0 1z"]},
        onClick: HandleOnSaveAs
    }
    
    // Create the editor
    editor = new JSONEditor({
        target: div,
        props: {
            onChange: HandleOnChange,
            onRenderMenu: HandleOnRenderMenu
        }
    });

    // Handles the onRenderMenu event to add custom buttons
    function HandleOnRenderMenu(items, context){

        // reset the readOnly property if it has been set to trigger re-rendering the menu
        if (context.readOnly === true){
            editor.updateProps({
                readOnly: false
            })
        }

        if (context.mode === 'table'){
            return items
        }

        saveButton.disabled = !canBeSaved

        items[items.length - 1] = {type: "separator"}
        items.push(openButton)
        items.push(saveButton)
        items.push(saveAsButton)

        return items
    }

    // Handles the change event from the editor
    function HandleOnChange() {
        SetValue("0")
        canBeSaved = true
    }

    // Handles the click on the 'open' button
    function HandleOnOpen() {
        SetValue("1")
        editor.focus()
    }

    // Handles the click on the 'save' button
    function HandleOnSave() {
        SetValue("2" + JsonToString())
        editor.focus()
    }

    // Handles the click on the 'save as' button
    function HandleOnSaveAs() {
        SetValue("3" + JsonToString())
        editor.focus()
    }

    // Returns the formatted json as string 
    function JsonToString(){
        return JSON.stringify(editor.get().json, null, "\t") ?? editor.get().text
    }

    // Sets a new value on the interface element
    function SetValue(value) {
        button.setAttribute("value", value)
        button.dispatchEvent(event)
    }
}
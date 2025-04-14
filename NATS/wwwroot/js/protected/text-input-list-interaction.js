"strict mode";

class TextInputListController {
    /**
     * @param {HTMLElement} listContainerElement
     * @param {string} inputContainerElementTemplate
     */
    constructor(listContainerElement, inputContainerElementTemplate) {
        this.listContainerElement = listContainerElement;
        
        // Configure all existing input container elements
        let nextIndex = 0;
        Array.from(listContainerElement.children).forEach(inputContainerElement => {
            this.configureInputContainer(inputContainerElement);
            nextIndex += 1;
        });

        /**
         * @type {HTMLButtonElement}
         */
        this.addButtonElement = document.getElementById(
            listContainerElement.getAttribute("add-button-id"));
        // Add event handler for add button
        this.addButtonElement.addEventListener("click", () => {
            let newInputContainerElementTemplate = inputContainerElementTemplate
                .replaceAll("{index}", nextIndex);
            listContainerElement.insertAdjacentHTML(
                'beforeend',
                newInputContainerElementTemplate);
            let addedInputContainerElement = listContainerElement.lastElementChild;
            this.configureInputContainer(addedInputContainerElement);
            nextIndex += 1;
        });
        
    }

    /**
     * @param {HTMLElement} inputContainerElement 
     */
    configureInputContainer(inputContainerElement) {
        /**
         * @type {HTMLInputElement}
         */
        let idInputElement = document.getElementById(
            inputContainerElement.getAttribute("id-input-element-id"));
        
        /**
         * @type {HTMLInputElement}
         */
        let contentInputElement = document.getElementById(
            inputContainerElement.getAttribute("content-input-element-id"));
        
        /**
         * @type {HTMLInputElement}
         */
        let isDeletedInputElement = document.getElementById(
            inputContainerElement.getAttribute("is-deleted-input-element-id"));
        
        /**
         * @type {HTMLButtonElement}
         */
        let deleteButtonElement = document.getElementById(
            inputContainerElement.getAttribute("delete-button-element-id"));
        /** Hide the entire input container when delete button in this container
         * is clicked, container and setting the value of content input elements
         * to null in order to optimize the data transfered to the server. */
        deleteButtonElement.addEventListener("click", () => {
            contentInputElement.value = "";
            isDeletedInputElement.value = true;
            inputContainerElement.style.display = "none";
        });
    }
}
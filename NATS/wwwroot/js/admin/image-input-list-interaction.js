class ImageInputListController {
    /**
     * @param {HTMLElement} listContainerElement
     * @param {string} newInputContainerElementTemplate
     */
    constructor(
            listContainerElement,
            newInputContainerElementTemplate) {
        this.listContainerElement = listContainerElement;
        
        /**
         * @type {HTMLButtonElement}
         */
        let addButtonElement = document.getElementById(
            listContainerElement.getAttribute("add-button-element-id"));

        /**
         * @type {Number}
         */
        this.nextIndex = 0;
        this.configureAllInputElementsChildren();

        addButtonElement.addEventListener("click", () => {
            /** Insert the index if this input container element to the template
              * and create new DOM by the template. */ 
            let inputContainerElementTemplate = newInputContainerElementTemplate
                .replaceAll("{index}", this.nextIndex);
            listContainerElement.insertAdjacentHTML("beforeend", inputContainerElementTemplate);

            /**
             * @type {HTMLElement}
             * Retrieve the input container element that has just been created and
             * hide it until user selects a file for its file input element.
             */
            let inputContainerElement = listContainerElement.lastElementChild;
            this.configureInputElementChildren(inputContainerElement);
            inputContainerElement.style.display = "none";
            
            // Append the input container element to the list container element
            listContainerElement.appendChild(inputContainerElement);

            /**
             * @type {HTMLInputElement}
             * Show file input dialog
             */
            let fileInputElement = document.getElementById(
                inputContainerElement.getAttribute("file-input-element-id"));
            fileInputElement.click();

            this.nextIndex += 1;
        });
    }

    /**
     * Add event listeners to elements in all input container elements
     */
    configureAllInputElementsChildren() {
        this.nextIndex = 0;
        Array.from(this.listContainerElement.children).forEach(inputContainerElement => {
            this.configureInputElementChildren(inputContainerElement);
            this.nextIndex += 1;
        });
    }

    /**
     * @param {HTMLElement} inputContainerElement
     * Add event listeners to elements in given input container element
     */
    configureInputElementChildren(inputContainerElement) {
        /**
         * @type {HTMLInputElement}
         */
        let idInputElement = document.getElementById(
            inputContainerElement.getAttribute("id-input-element-id"));

        /**
         * @type {HTMLInputElement}
         */
        let fileInputElement = document.getElementById(
            inputContainerElement.getAttribute("file-input-element-id"));

        /**
         * @type {HTMLInputElement}
         */
        let isDeletedInputElement = document.getElementById(
            inputContainerElement.getAttribute("is-deleted-input-element-id"));

        /**
         * @type {HTMLInputElement}
         */
        let urlInputElement = document.getElementById(
            inputContainerElement.getAttribute("url-input-element-id"));
            
        /**
         * @type {HTMLImageElement}
         */
        let imageElement = document.getElementById(
            inputContainerElement.getAttribute("image-element-id"));

        /**
         * @type {HTMLButtonElement}
         */
        let deleteButtonElement = document.getElementById(
            inputContainerElement.getAttribute("delete-button-element-id"));

        // When clicking the delete button, set default image source to the image element
        deleteButtonElement.onclick = () => {
            isDeletedInputElement.value = "True";
            fileInputElement.value = "";
            urlInputElement.value = "";
            inputContainerElement.style.display = "none";
        };
        
        // When user has selected an image, display that image as an thumbnail to the image element
        fileInputElement.onchange = () => {
            if (fileInputElement.files && fileInputElement.files[0]) {
                let reader = new FileReader()
                reader.onload = (event) => {
                    imageElement.setAttribute("src", event.target.result.toString());
                }
                reader.readAsDataURL(fileInputElement.files[0]);
                inputContainerElement.style = "";
            }
        }
            
        /** When user aborts file selection, change is-deleted input element value to
          * true to indicate that this image is deleted (for filtering process on server-side) */
        fileInputElement.addEventListener("cancel", () => {
            isDeletedInputElement.value = true;
        })
    }
}


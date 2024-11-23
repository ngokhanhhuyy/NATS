class PhotoFileInputButtonPair {
    /**
     * @param {HTMLButtonElement} editButton
     * @param {string} defaultImageSource
     */
    constructor(editButton, defaultImageSource) {
        this.editButton = editButton;
        this.defaultImageSource = defaultImageSource;
        
        /**
         * Retrieve the related elements:
         * - file input => input which holds the data of the selected image
         * - hidden original url input => input which holds the original (pre-changed) image url
         * - changed input => input which hold boolean value to determine if user has changed the image
         * - delete button (optional)
         * - image element
         */
        
        /**
         * @type {HTMLInputElement}
         */
        this.fileInput = document.getElementById(editButton.getAttribute("file-input-id"));
        
        /**
         * @type {HTMLInputElement}
         */
        this.hiddenUrlInput = document.getElementById(editButton.getAttribute("hidden-url-input-id"));

        /**
         * @type {HTMLInputElement}
         */
        this.changedInput = document.getElementById(editButton.getAttribute("changed-input-id"));

        /**
         * @type {HTMLButtonElement | null}
         */
        this.deleteButton = document.getElementById(
            editButton.getAttribute("delete-button-id"));

        /**
         * @type {HTMLImageElement}
         */
        this.image = document.getElementById(
            editButton.getAttribute("img-id"));

        /**
         * @type {HTMLElement | null}
         */
        this.container = document.getElementById(editButton.getAttribute("container-id"));

        // When clicking the edit button, also click hidden file input element programatically
        this.editButton.onclick = () => {
            this.fileInput.click();
        };

        // When clicking the delete button, set default image source to the image element
        if (this.deleteButton) {
            this.deleteButton.onclick = () => {
                this.changedInput.value = true;
                this.fileInput.value = null;
                if (this.container >= 0) {
                    this.changeImageSource();
                    this.deleteButton.style.visibility = "hidden";
                } else {
                    this.container.style.display = "none";
                }
            }
        }

        // When the value of file input element changes, change the thumbnail src attribute
        this.fileInput.onchange = () => {
            this.changeImageSource();
            this.changeDeleteButtonVisibility();
            this.changedInput.value = true;
        }
        
        // Show or hide the delete button on initialization
        this.changeDeleteButtonVisibility();
    }

    /**
     * Change image element source which is the value of the file input
     */
    changeImageSource() {
        if (this.fileInput.files && this.fileInput.files[0]) {
            let reader = new FileReader()
            reader.onload = (event) => {
                this.image.setAttribute("src", event.target.result.toString());
            }
            reader.readAsDataURL(this.fileInput.files[0]); 
        } else {
            this.image.setAttribute("src", this.defaultImageSource);
        }
    }

    /**
     * Show or hide delete button based on if the file input holds any value
     */
    changeDeleteButtonVisibility() {
        if (this.deleteButton) {
            if (this.fileInput.files && this.fileInput.files[0] || this.hiddenUrlInput.value) {
                this.deleteButton.style.visibility = "visible";
            } else {
                this.deleteButton.style.visibility = "hidden";
            }
        }
    }
}
/**
 * @typedef {"thumbnailAdded" | "thumbnailCleared" | "thumbnailUploading"} Mode
 */

/**
 * @typedef {{
 *      data: {
 *          url: string;
 *          thumb: { url: string };
 *          success: boolean;
 *          status: number;
 *      }
 *  }} UploadingResponseJSON
 */

/**
 * Creates a controller that controls the input element for uploading thumbnail image.
 * 
 * @param {string} imgbbApiKey The api key to ImgBB api.
 * @param {string | HTMLDivElement} container The query selector to the container
 * element.
 */
function createThumbnailInputController(imgbbApiKey, container) {
    const apiKey = imgbbApiKey;
    const /** @type {HTMLDivElement} */ containerElement = typeof container === "string"
        ? document.querySelector(container)
        : container;
    
    /** @type {HTMLButtonElement} */
    const buttonElement = containerElement.querySelector(".click-area");
    
    /** @type {HTMLInputElement} */
    const fileInputElement = containerElement.querySelector(`input[type="file"]`);
    
    /** @type {HTMLInputElement} */
    const hiddenInputElement = containerElement.querySelector(`input[type="hidden"]`);
    
    /** @type {HTMLDivElement} */
    const thumbnailPreviewElement = containerElement.querySelector(".thumbnail-preview");
    
    /** @type {HTMLDivElement} */
    const thumbnailAddingIndicatorElement = containerElement
        .querySelector(".thumbnail-adding-indicator");
    
    /** @type {HTMLDivElement} */
    const thumbnailUploadingInidicatorElement = containerElement
        .querySelector(".thumbnail-loading-indicator");
            
    /** @type {HTMLImageElement} */
    const thumbnailPreviewImageElement = thumbnailPreviewElement.querySelector("img");
    
    /** @type {HTMLButtonElement} */
    const thumbnailClearButtonElement = thumbnailPreviewElement.querySelector("button");

    /** @type {HTMLDivElement} */
    const thumbnailAddingIndicatorTextElement = thumbnailAddingIndicatorElement
        .querySelector(".adding-indicator-text");

    /** @type {HTMLDivElement} */
    const validationMessageContainerElement = thumbnailAddingIndicatorElement
        .querySelector(".validation-message-container");

    /** @type {HTMLSpanElement} */
    const validationMessageElement = thumbnailAddingIndicatorElement
        .querySelector(".validation-message");
    
    /** @type {HTMLDivElement} */
    const modalElement = containerElement.querySelector(".modal");
    
    /** @type {HTMLDivElement} */
    const modalBodyElement = modalElement.querySelector(".modal-body");
    
    /** @type {bootstrap.Modal} */
    const modal = new window.bootstrap.Modal(modalElement);
    
    buttonElement.addEventListener("click", () => {
        fileInputElement.click();
    });
    
    thumbnailClearButtonElement.addEventListener("click", clearThumbnail);
    
    fileInputElement.addEventListener("change", async (event) => {
        /** @type {HTMLInputElement} */
        const inputElement = event.target;
        if (!inputElement.files || !inputElement.files[0]) {
            clearThumbnail();
        }
        
        const file = inputElement.files[0];
        try {
            await validateFile(file);
            showValidationErrorMessage(null);
            switchMode("thumbnailUploading");
            const uploadingResult = await uploadFile(file);
            switchMode("thumbnailAdded");
            thumbnailPreviewImageElement.setAttribute("src", uploadingResult.thumbnailUrl);
        } catch (error) {
            if (typeof error !== "string") {
                throw error;
            }

            showValidationErrorMessage("File phải có kích thước nhỏ hơn 3MB");
        }
    });

    /**
     * Clears thumbnail url and images, shows the thumbnail adding elements.
     * 
     * @param {Event} event
     */
    function clearThumbnail(event) {
        event.preventDefault();
        event.stopPropagation();
        switchMode("thumbnailCleared");
        thumbnailPreviewImageElement.removeAttribute("src");
        hiddenInputElement.value = "";
    }

    /**
     * Validate if the file is valid by type and by size. The given file must be a valid
     * PNG or JPEG/JPG file and its size must be equal or less than 3MB.
     * 
     * @param {File} file
     * @returns {Promise<void>}
     * 
     * @throws "FileTooLarge" When the given file size exceeds 3MB.
     * @throws "InvalidFileType" When the given file type is not any of PNG or JPEG/JPG.
     */
    async function validateFile(file) {
        return new Promise((resolve, reject) => {
            if (file.size > 1.5 * 1024 * 1024) {
                reject("File phải có kích thước nhỏ hơn 3MB");
            }
        
            const reader = new FileReader();
            reader.onload = () => {
                const /** @type {ArrayBuffer} */ readerResult = reader.result;
                const arr = new Uint8Array(readerResult);

                // PNG signature: 89 50 4E 47 0D 0A 1A 0A
                const isPng = arr[0] === 0x89 &&
                    arr[1] === 0x50 &&
                    arr[2] === 0x4E &&
                    arr[3] === 0x47;

                // JPEG signature: FF D8 ... FF D9
                const isJpeg = arr[0] === 0xFF && arr[1] === 0xD8;

                if (!isPng && !isJpeg) {
                    reject("File phải là ảnh PNG hoặc JPEG/JPG");
                }
                
                resolve();
            };
            
            reader.readAsArrayBuffer(file.slice(0, 4));
        });
    }

    /**
     * Uploads the specified file to ImgBB api and retrieve the image url.
     * 
     * @param file
     * @returns {Promise<{ thumbnailUrl: string; imageUrl: string }>} A {@link Promise} which
     * resolves to an object containing the preview thumbnail url and the full size image url.
     * 
     * @throws "InvalidFileType" Throws when the uploaded file type is invalid.
     * @throws Error Throws when the uploading process failed with undefined reason.
     */
    async function uploadFile(file) {
        const formData = new FormData();
        formData.append("image", file);
        const response = await fetch(`https://api.imgbb.com/1/upload?key=${apiKey}`, {
            method: "post",
            body: formData
        });
        
        const /** @@type {UploadingResponseJSON} */ json = await response.json();
        if (response.status === 400) {
            if (json.data.status === 301) {
                throw "InvalidFileType";
            }
            
            throw Error(json);
        }
        
        return {
            thumbnailUrl: json.data.thumb.url,
            imageUrl: json.data.url
        }
    }

    /**
     * Shows validation error messsage and adds style to the container which indicates that
     * there is error. If value for {@link message} is null or empty, the validation error
     * message and the style will be cleared.
     * 
     * @param {string | null} message 
     */
    function showValidationErrorMessage(message) {
        if (message) {
            containerElement.classList.add("error");
            thumbnailAddingIndicatorTextElement.classList.add("d-none");
            validationMessageContainerElement.classList.remove("d-none");
            validationMessageElement.textContent = message;
            return;
        }

        containerElement.classList.remove("error");
        thumbnailAddingIndicatorTextElement.classList.remove("d-none");
        validationMessageContainerElement.classList.add("d-none");
        validationMessageElement.textContent = "";
    }

    /**
     * Show the validation error notification modal with the specified texts.
     * 
     * @param {string[]} texts An array of strings, representing the sentences in the
     * messages that are shown in the modal body.
     */
    function showModal(texts) {
        modalBodyElement.replaceChildren();
        for (const text of texts) {
            const spanElement = document.createElement("span");
            spanElement.textContent = text;
            modalBodyElement.appendChild(spanElement);
        }
        
        modal.show();
    }

    /**
     * Switch visible indicator corresponding to the given mode.
     * 
     * @param {Mode} mode
     */
    function switchMode(mode) {
        /** @@type {{ [key: Mode]: HTMLDivElement }} */
        const elementForModes = {
            thumbnailAdded: thumbnailPreviewElement,
            thumbnailCleared: thumbnailAddingIndicatorElement,
            thumbnailUploading: thumbnailUploadingInidicatorElement
        }
        
        for (const [evaluatingMode, element] of Object.entries(elementForModes)) {
            if (evaluatingMode === mode) {
                element.classList.remove("d-none");
            } else {
                element.classList.add("d-none");
            }
        }
    }
}
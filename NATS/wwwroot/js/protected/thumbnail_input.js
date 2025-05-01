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
    let /** @type {HTMLDivElement} */ containerElement;
    if (typeof container === "string") {
        containerElement = document.querySelector(container);
    } else {
        containerElement = container;
    }
    
    /** @type {HTMLButtonElement} */
    const buttonElement = containerElement.querySelector(".click-area");
    
    /** @type {HTMLInputElement} */
    const fileInputElement = containerElement.querySelector(`input[type="file"]`);
    
    /** @type {HTMLInputElement} */
    const hiddenInputElement = containerElement.querySelector(`input[type="hidden"]`);
    
    /** @type {HTMLDivElement} */
    const thumbnailPreviewElement = containerElement.querySelector(".thumbnail-preview");
    
    /** @type {HTMLDivElement} */
    const thumbnailAddingIndicator = containerElement
        .querySelector(".thumbnail-adding-indicator");
    
    /** @type {HTMLDivElement} */
    const thumbnailUploadingInidicator = containerElement
        .querySelector(".thumbnail-loading-indicator");
            
    /** @type {HTMLImageElement} */
    const thumbnailPreviewImageElement = thumbnailPreviewElement.querySelector("img");
    
    /** @type {HTMLButtonElement} */
    const thumbnailClearButtonElement = thumbnailPreviewElement.querySelector("button");
    
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
    
    fileInputElement.addEventListener("change", async () => {
        if (!fileInputElement.files || !fileInputElement.files[0]) {
            clearThumbnail();
        }
        
        const file = fileInputElement.files[0];
        try {
            await validateFile(file);
            switchMode("thumbnailUploading");
            const uploadingResult = await uploadFile(file);
            switchMode("thumbnailAdded");
            thumbnailPreviewImageElement.setAttribute("src", uploadingResult.imageUrl);
        } catch (error) {
            if (typeof error !== "string") {
                throw error;
            }
            
            if (error === "FileTooLarge") {
                showModal([
                    "File có kích thước quá lớn",
                    "Hãy đảm bảo rằng file bạn muốn upload có kích thước nhỏ hơn 3MB."
                ]);
                
                return;
            }
            
            if (error === "InvalidFileType") {
                showModal([
                    "Kiểu file không hợp lệ",
                    "Hãy đảm bảo rằng file bạn muốn upload có kiểu PNG hoặc JPEG/JPG"
                ]);
            }
        }
    });

    /**
     * Clears thumbnail url and images, shows the thumbnail adding elements.
     */
    function clearThumbnail() {
        switchMode("thumbnailCleared");
        thumbnailPreviewImageElement.setAttribute("src", undefined);
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
            console.log(file.size / (1024 * 1024));
            reject("FileTooLarge");
            // if (file.size > 1.5 * 1024 * 1024) {
            //     reject("FileTooLarge");
            // }
        
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
                    reject("InvalidFileType");
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
            thumbnailCleared: thumbnailAddingIndicator,
            thumbnailUploading: thumbnailUploadingInidicator
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
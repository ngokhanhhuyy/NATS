import { ref, computed, createApp } from "vue";

/**
 * Creates a controller that controls the input element for uploading thumbnail image.
 * 
 * @param {string | null} initialThumbnailUrl The initial URL of the thumbnail.
 * @param {string} imgbbApiKey The api key to ImgBB api.
 * @param {string | HTMLDivElement} container The query selector to the container element.
 */
export function createThumbnailInputController(initialThumbnailUrl, imgbbApiKey, container) {
    const app = createApp({
        data() {
            return {
                thumbnailUrl: initialThumbnailUrl,
            }
        },
    });
}

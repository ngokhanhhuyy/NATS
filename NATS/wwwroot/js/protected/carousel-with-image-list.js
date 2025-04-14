"use strict";
class CarouselController {
    /**
     * @param {HTMLElement} carouselElement
     * @param {NodeListOf<HTMLImageElement>} imageElements
     */
    constructor(carouselElement, imageElements) {
        const carousel = new bootstrap.Carousel(carouselElement);
        this.imageElements = imageElements;
        // Change carousel index on image element clicked
        this.imageElements.forEach(imageElement => {
            imageElement.addEventListener("click", () => {
                /**
                 * @type {String}
                 */
                let imageElementIndex = imageElement.getAttribute("image-index");
                carousel.to(imageElementIndex);
            })
        })
        // Set initial states
        this.changeImageElementsStates(null, 0);
        // Change image elements states on carousel index changed
        carouselElement.addEventListener("slide.bs.carousel", (event) => {
            this.changeImageElementsStates(event.from, event.to);
        });
    }

    /** 
     * @param {number} from
     * @param {number} to
     */
    changeImageElementsStates(from, to) {
        this.imageElements.forEach(imageElement => {
            /**
             * @type {string}
             */
            let imageIndex = imageElement.getAttribute("image-index");
            if (imageIndex == from) {
                imageElement.classList.remove("border-primary");
                imageElement.classList.remove("bg-primary-subtle");
                imageElement.classList.remove("shadow");
                imageElement.classList.add("shadow-sm");
            } else if (imageIndex == to) {
                imageElement.classList.add("border-primary");
                imageElement.classList.add("bg-primary-subtle");
                imageElement.classList.add("shadow");
                imageElement.classList.remove("shadow-sm");
            }
        });
    }
}
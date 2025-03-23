import { Carousel } from "bootstrap";

document.addEventListener("DOMContentLoaded", () => {
    const carouselElement = document.getElementById("slider");
    const carousel = new Carousel(carouselElement)
    console.log(carousel);
});
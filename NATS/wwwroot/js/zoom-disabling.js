import "bootstrap";

// Prevent zooming with more than one finger
document.addEventListener("touchstart", function(e) {
    if (e.touches.length > 1) {
        e.preventDefault(); // Prevent zoom
    }
}, { passive: false });

// Prevent pinch zooming with gestures
document.addEventListener("gesturestart", function(e) {
    e.preventDefault(); // Prevent zoom gesture
}, { passive: false });
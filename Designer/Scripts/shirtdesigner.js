
const canvas = document.getElementById('my-canvas');
let img = null;
const ctx = canvas.getContext('2d');
let offsetX = 0;
let offsetY = 0;
let scaledWidth = 0;
let scaledHeight = 0;

const resetCanvas = () => {
    ctx.clearRect(0, 0, canvas.width, canvas.height); // Clear the canvas
    if (img) {
        ctx.drawImage(img, offsetX, offsetY, scaledWidth, scaledHeight);
    }
}

//Add image to canvas
function imageClicked(src) {
    modal.style.display = "none";
    closeModal();
    img = new Image();
    img.src = src;
    img.onload = () => {
        scaleImageToFitCanvas();
    };
}

// Function to scale the image to fit the canvas
function scaleImageToFitCanvas() {
    // Get the aspect ratio of the image
    const imgAspectRatio = img.width / img.height;
    // Get the aspect ratio of the canvas
    const canvasAspectRatio = canvas.width / canvas.height;

    // Determine whether to scale based on width or height by comparing aspect ratios
    if (imgAspectRatio > canvasAspectRatio) {
        // Image is wider in proportion to the canvas
        scaledWidth = canvas.width; // Use the full width of the canvas
        scaledHeight = scaledWidth / imgAspectRatio; // Adjust height to maintain aspect ratio
    } else {
        // Image is taller in proportion to the canvas
        scaledHeight = canvas.height; // Use the full height of the canvas
        scaledWidth = scaledHeight * imgAspectRatio; // Adjust width to maintain aspect ratio
    }

    offsetX = (canvas.width - scaledWidth) / 2; // Center image horizontally
    offsetY = (canvas.height - scaledHeight) / 2; // Center image vertically
    resetCanvas();
}

let draggable = false;

canvas.onmousedown = (e) => {
    const clickX = e.layerX;
    const clickY = e.layerY;
    // Calculate middle position of the image
    const imageMiddleX = offsetX + scaledWidth / 2;
    const imageMiddleY = offsetY + scaledHeight / 2;
    // Check if the click occurred within the boundaries of the image
    if (clickX >= offsetX && clickX <= offsetX + scaledWidth && clickY >= offsetY && clickY <= offsetY + scaledHeight) {
        // Calculate offset from the middle of the image
        offsetX += clickX - imageMiddleX;
        offsetY += clickY - imageMiddleY;
        resetCanvas();
        draggable = true;
    }

    window.addEventListener('keydown', (e) => {
        if (e.key === 'Delete') {
            img = null;
            resetCanvas();
        }
    });
}

canvas.onmousemove = (e) => {
    if (draggable) {
        offsetX += e.movementX;
        offsetY += e.movementY;
        resetCanvas();
    }
}
canvas.onmouseup = (e) => {
    draggable = false;
}
canvas.onmouseout = (e) => {
    draggable = false;
}


//Szöveg hozzáadása

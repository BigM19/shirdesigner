//Save merged image
function mergeAndSaveCanvas() {
    const offScreenCanvas = document.createElement('canvas');
    const offCtx = offScreenCanvas.getContext('2d');

    const backgroundImage = document.getElementById('shirt-image');

    // Assuming backgroundImage is loaded and available
    offScreenCanvas.width = backgroundImage.width;
    offScreenCanvas.height = backgroundImage.height;

    // Draw the background image first
    offCtx.drawImage(backgroundImage, 0, 0, 600, 600);

    // Then draw the display canvas content over it
    const displayCanvas = document.getElementById('my-canvas');
    const startX = 160;  // Calculated X coordinate
    const startY = 150;  // Calculated Y coordinate
    offCtx.drawImage(displayCanvas, startX, startY);

    // Export to Image
    const imageDataURL = offScreenCanvas.toDataURL('image/png');

    var xhr = new XMLHttpRequest();
    xhr.open('POST', '@Url.Action("SaveDesign", "DesignableProduct", new { id = Model.ItemId })', true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(JSON.stringify({ imageData: imageDataURL }));

    window.location.reload();
}
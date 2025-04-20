document.getElementById('gameCodeTemplate').addEventListener('input', function () {
    let gameCode = document.getElementById('gameCodeTemplate').value;
    renderGameOnCanvas(gameCode);  // Function to visualize the game on canvas
});

function renderGameOnCanvas(gameCode) {
    let canvas = document.getElementById('gameCanvas');
    let ctx = canvas.getContext('2d');

    // Reset the canvas
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    // Render based on the code (for now, we simulate it with some static content)
    ctx.fillStyle = "blue";
    ctx.fillRect(50, 50, 100, 100);  // Example: Just draw a square as placeholder

    // Extend with game rendering logic based on the user's code
}


document.getElementById('saveGameCode').addEventListener('click', function () {
    let modifiedCode = document.getElementById('gameCodeTemplate').value;
    let gameTemplateId = 1;  // This should be dynamic (from the template the user is working on)

    $.ajax({
        type: "POST",
        url: "/Game/SaveUserGameCode",
        data: {
            gameTemplateId: gameTemplateId,
            modifiedCode: modifiedCode
        },
        success: function (response) {
            alert("Game code saved successfully!");
        },
        error: function (response) {
            alert("Error saving game code.");
        }
    });
});

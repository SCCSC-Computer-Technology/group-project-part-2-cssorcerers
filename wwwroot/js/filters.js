document.getElementById('searchInput').addEventListener('keyup', function () {
    const value = this.value.toLowerCase();
    const rows = document.querySelectorAll('#teamsBody tr');

    rows.forEach(row => {
        const teamName = row.children[0].textContent.toLowerCase();
        row.style.display = teamName.includes(value) ? '' : 'none';
    });
});

function sortTable() {
    const sortBy = document.getElementById('sortBy').value;
    const tableBody = document.getElementById('teamsBody');
    const rows = Array.from(tableBody.querySelectorAll('tr'));

    if (!sortBy) return;

    const columnMap = {
        'TeamName': 0,
        'Points': 1,
        'Wins': 2,
        'Losses': 3,
        'GoalsFor': 5,
        'GoalsAgainst': 6
    };

    const columnIndex = columnMap[sortBy];

    rows.sort((a, b) => {
        const aText = a.children[columnIndex].textContent;
        const bText = b.children[columnIndex].textContent;

        // Numeric sort if not team name
        if (sortBy === 'TeamName') {
            return aText.localeCompare(bText);
        } else {
            return parseInt(bText) - parseInt(aText);
        }
    });

    rows.forEach(row => tableBody.appendChild(row));
}

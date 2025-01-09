async function fetchSuggestions() {
    const searchTerm = document.getElementById('searchString').value;
    const suggestionsList = document.getElementById('suggestions');
    suggestionsList.innerHTML = '';

    if (searchTerm.length < 3) {
        return;
    }

    const response = await fetch(`/Home/HotelSuggestions?searchTerm=${searchTerm}`);
    const suggestions = await response.json();

    suggestions.forEach(suggestion => {
        const li = document.createElement('li');
        li.classList.add('list-group-item');
        li.textContent = `${suggestion.name} - ${suggestion.location} - ${suggestion.price} €`;
        li.onclick = () => {
            window.location.href = `/Home/BookHotel?id=${suggestion.id}`;
        };
        suggestionsList.appendChild(li);
    });
}

// isLoggedIn is now defined in the HTML file

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
        li.innerHTML = `
            <img src="${suggestion.imageUrl}" alt="${suggestion.name}" style="width: 50px; height: 50px; margin-right: 10px;">
            ${suggestion.name} - ${suggestion.location} - ${suggestion.price} €
        `;
        li.onclick = () => {
            if (isLoggedIn === 'true') {
                window.location.href = `/Home/BookHotel?id=${suggestion.id}`;
            } else {
                const hotelCard = document.getElementById(`hotel-${suggestion.id}`);
                if (hotelCard) {
                    hotelCard.scrollIntoView({ behavior: 'smooth' });
                }
            }
        };
        suggestionsList.appendChild(li);
    });
}
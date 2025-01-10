function changeCurrency() {
    const currency = document.getElementById("currencySelector").value;
    document.cookie = `currency=${currency}; path=/`;

    // Uppdatera priserna på sidan
    updatePrices(currency);
}

function updatePrices(currency) {
    const priceElements = document.querySelectorAll(".price");
    priceElements.forEach(element => {
        const basePrice = parseFloat(element.getAttribute("data-base-price"));
        let convertedPrice = basePrice;

        switch (currency) {
            case "dollar":
                convertedPrice = basePrice * 1.1; // Exempelkonvertering
                element.innerHTML = `$${convertedPrice.toFixed(2)}`;
                break;
            case "pound":
                convertedPrice = basePrice * 0.85; // Exempelkonvertering
                element.innerHTML = `£${convertedPrice.toFixed(2)}`;
                break;
            case "sek":
                convertedPrice = basePrice * 10; // Exempelkonvertering
                element.innerHTML = `${convertedPrice.toFixed(2)} kr`;
                break;
            default:
                element.innerHTML = `€${convertedPrice.toFixed(2)}`;
                break;
        }
    });
}

// Kör updatePrices när sidan laddas
document.addEventListener("DOMContentLoaded", () => {
    const currency = getCookie("currency") || "euro";
    document.getElementById("currencySelector").value = currency;
    updatePrices(currency);
});

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}
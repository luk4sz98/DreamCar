function initAutocomplete() {
    var input = document.getElementById("localization");
    var restrictOptions = {
        componentRestrictions: { country: 'pl' },
        types: ["(regions)"]
    };
    new google.maps.places.Autocomplete(input, restrictOptions);
}

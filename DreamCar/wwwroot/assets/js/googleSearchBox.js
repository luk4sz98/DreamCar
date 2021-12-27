function initAutocomplete() {
    var input = document.getElementById("localization");
    var restrictOptions = {
        fields: ["address_components", "geometry", "name"],
        strictBounds: true,
        componentRestrictions: { country: 'pl' },
        types: ["(regions)"]
    };
    let autocomplete = new google.maps.places.Autocomplete(input, restrictOptions);
    google.maps.event.addListener(autocomplete, 'place_changed', function () {
        $("#localization").trigger("click")
    });
}

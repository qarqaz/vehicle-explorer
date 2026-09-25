const makeInput =
    document.getElementById("makeInput");

const makeDropdown =
    document.getElementById("makeDropdown");

const clearMakeButton =
    document.getElementById("clearMakeButton");


const yearSelect =
    document.getElementById("yearSelect");


const vehicleTypeInput =
    document.getElementById("vehicleTypeInput");

const vehicleTypeDropdown =
    document.getElementById("vehicleTypeDropdown");

const clearVehicleTypeButton =
    document.getElementById("clearVehicleTypeButton");


const searchButton =
    document.getElementById("searchButton");

const modelsContainer =
    document.getElementById("modelsContainer");

const message =
    document.getElementById("message");


const apiUrlInput =
    document.getElementById("apiUrlInput");

const sendApiButton =
    document.getElementById("sendApiButton");

const apiResponse =
    document.getElementById("apiResponse");

const apiStatus =
    document.getElementById("apiStatus");

const copyJsonButton =
    document.getElementById("copyJsonButton");


let makes = [];
let vehicleTypes = [];

let selectedMakeId = null;
let lastApiJson = null;


// =====================================================
// Initialize
// =====================================================

document.addEventListener("DOMContentLoaded", async () => {

    loadYears();

    await loadMakes();

});


// =====================================================
// Makes
// =====================================================

async function loadMakes() {

    try {

        const response = await fetch("/api/makes");

        if (!response.ok) {
            throw new Error("Failed to load vehicle makes.");
        }

        makes = await response.json();

        renderMakeDropdown(makes);

    }
    catch (error) {

        showMessage(error.message);

    }

}


function renderMakeDropdown(items) {

    makeDropdown.innerHTML = "";

    if (items.length === 0) {

        addEmptyItem(
            makeDropdown,
            "No makes found."
        );

        return;

    }

    items.forEach(make => {

        const item =
            document.createElement("div");

        item.className = "dropdown-item";

        item.textContent = make.name;

        item.addEventListener("mousedown", event => {

            event.preventDefault();

            selectMake(make);

        });

        makeDropdown.appendChild(item);

    });

}


function selectMake(make) {

    selectedMakeId = make.id;

    makeInput.value = make.name;

    makeDropdown.classList.remove("open");

    updateClearButtons();

    clearVehicleType();

    clearResults();

    loadVehicleTypes(make.id);

    updateSearchButton();

}


// =====================================================
// Make Events
// =====================================================

makeInput.addEventListener("focus", () => {

    const filteredMakes =
        filterByName(
            makes,
            makeInput.value
        );

    renderMakeDropdown(filteredMakes);

    makeDropdown.classList.add("open");

});


makeInput.addEventListener("click", () => {

    const filteredMakes =
        filterByName(
            makes,
            makeInput.value
        );

    renderMakeDropdown(filteredMakes);

    makeDropdown.classList.add("open");

});


makeInput.addEventListener("input", () => {

    selectedMakeId = null;

    clearVehicleType();

    clearResults();

    const filteredMakes =
        filterByName(
            makes,
            makeInput.value
        );

    renderMakeDropdown(filteredMakes);

    makeDropdown.classList.add("open");

    updateClearButtons();

    updateSearchButton();

});


clearMakeButton.addEventListener("click", () => {

    makeInput.value = "";

    selectedMakeId = null;

    clearVehicleType();

    clearResults();

    renderMakeDropdown(makes);

    makeDropdown.classList.add("open");

    updateClearButtons();

    updateSearchButton();

    makeInput.focus();

});


// =====================================================
// Years
// =====================================================

function loadYears() {

    const currentYear =
        new Date().getFullYear();

    for (
        let year = currentYear;
        year >= 1980;
        year--
    ) {

        const option =
            document.createElement("option");

        option.value = year;
        option.textContent = year;

        yearSelect.appendChild(option);

    }

}


yearSelect.addEventListener(
    "change",
    updateSearchButton
);


// =====================================================
// Vehicle Types
// =====================================================

async function loadVehicleTypes(makeId) {

    try {

        vehicleTypeInput.disabled = true;

        const response = await fetch(
            `/api/makes/${makeId}/vehicle-types`
        );

        if (!response.ok) {

            throw new Error(
                "Failed to load vehicle types."
            );

        }

        vehicleTypes =
            await response.json();

        renderVehicleTypeDropdown(
            vehicleTypes
        );

        vehicleTypeInput.disabled = false;

    }
    catch (error) {

        showMessage(error.message);

    }

}


function renderVehicleTypeDropdown(items) {

    vehicleTypeDropdown.innerHTML = "";

    if (items.length === 0) {

        addEmptyItem(
            vehicleTypeDropdown,
            "No vehicle types found."
        );

        return;

    }

    items.forEach(vehicleType => {

        const item =
            document.createElement("div");

        item.className = "dropdown-item";

        item.textContent =
            vehicleType.name;

        item.addEventListener(
            "mousedown",
            event => {

                event.preventDefault();

                selectVehicleType(
                    vehicleType
                );

            }
        );

        vehicleTypeDropdown.appendChild(
            item
        );

    });

}


function selectVehicleType(vehicleType) {

    vehicleTypeInput.value =
        vehicleType.name;

    vehicleTypeDropdown
        .classList
        .remove("open");

    clearResults();

    updateClearButtons();

    updateSearchButton();

}


// =====================================================
// Vehicle Type Events
// =====================================================

vehicleTypeInput.addEventListener(
    "focus",
    () => {

        const filtered =
            filterByName(
                vehicleTypes,
                vehicleTypeInput.value
            );

        renderVehicleTypeDropdown(
            filtered
        );

        vehicleTypeDropdown
            .classList
            .add("open");

    }
);


vehicleTypeInput.addEventListener(
    "click",
    () => {

        const filtered =
            filterByName(
                vehicleTypes,
                vehicleTypeInput.value
            );

        renderVehicleTypeDropdown(
            filtered
        );

        vehicleTypeDropdown
            .classList
            .add("open");

    }
);


vehicleTypeInput.addEventListener(
    "input",
    () => {

        clearResults();

        const filtered =
            filterByName(
                vehicleTypes,
                vehicleTypeInput.value
            );

        renderVehicleTypeDropdown(
            filtered
        );

        vehicleTypeDropdown
            .classList
            .add("open");

        updateClearButtons();

        updateSearchButton();

    }
);


clearVehicleTypeButton.addEventListener(
    "click",
    () => {

        vehicleTypeInput.value = "";

        clearResults();

        renderVehicleTypeDropdown(
            vehicleTypes
        );

        vehicleTypeDropdown
            .classList
            .add("open");

        updateClearButtons();

        updateSearchButton();

        vehicleTypeInput.focus();

    }
);


// =====================================================
// Search Button State
// =====================================================

function updateSearchButton() {

    const validMake =
        selectedMakeId !== null;

    const validYear =
        yearSelect.value !== "";

    const validVehicleType =
        vehicleTypes.some(
            type =>
                normalize(type.name) ===
                normalize(
                    vehicleTypeInput.value
                )
        );

    searchButton.disabled =
        !validMake ||
        !validYear ||
        !validVehicleType;

}


// =====================================================
// Model Search
// =====================================================

searchButton.addEventListener(
    "click",
    async () => {

        clearResults();

        showMessage("Loading models...");

        const selectedVehicleType =
            vehicleTypes.find(
                type =>
                    normalize(type.name) ===
                    normalize(
                        vehicleTypeInput.value
                    )
            );

        if (
            !selectedMakeId ||
            !selectedVehicleType
        ) {

            showMessage(
                "Please select valid search criteria."
            );

            return;

        }

        const year =
            yearSelect.value;

        const vehicleType =
            encodeURIComponent(
                selectedVehicleType.name
            );

        const apiUrl =
            `/api/models?makeId=${selectedMakeId}` +
            `&year=${year}` +
            `&vehicleType=${vehicleType}`;

        /*
         * Keep the API Playground synchronized
         * with the request made by the normal UI.
         */
        apiUrlInput.value = apiUrl;

        try {

            const response =
                await fetch(apiUrl);

            if (!response.ok) {

                throw new Error(
                    "Failed to load vehicle models."
                );

            }

            const models =
                await response.json();

            displayModels(models);

        }
        catch (error) {

            showMessage(error.message);

        }

    }
);


// =====================================================
// Display Model Results
// =====================================================

function displayModels(models) {

    modelsContainer.innerHTML = "";

    if (models.length === 0) {

        showMessage(
            "No models found for the selected criteria."
        );

        return;

    }

    showMessage(
        `${models.length} model(s) found.`
    );

    models.forEach(model => {

        const card =
            document.createElement("div");

        card.className =
            "model-card";


        const title =
            document.createElement("h3");

        title.textContent =
            model.name;


        const make =
            document.createElement("p");

        make.textContent =
            model.makeName;


        const type =
            document.createElement("p");

        type.textContent =
            model.vehicleTypeName;


        card.appendChild(title);
        card.appendChild(make);
        card.appendChild(type);

        modelsContainer.appendChild(card);

    });

}


// =====================================================
// API Playground
// =====================================================

sendApiButton.addEventListener(
    "click",
    sendApiRequest
);


apiUrlInput.addEventListener(
    "keydown",
    event => {

        if (event.key === "Enter") {

            event.preventDefault();

            sendApiRequest();

        }

    }
);


document
    .querySelectorAll(".api-example")
    .forEach(button => {

        button.addEventListener(
            "click",
            () => {

                apiUrlInput.value =
                    button.dataset.url;

                sendApiRequest();

            }
        );

    });


async function sendApiRequest() {

    const url =
        apiUrlInput.value.trim();

    if (!url) {

        setApiStatus(
            "Enter an API path",
            "error"
        );

        return;

    }

    /*
     * Keep this playground scoped to our own API.
     * This prevents it from becoming an arbitrary
     * external URL requester.
     */
    if (!url.startsWith("/api/")) {

        setApiStatus(
            "Only /api/ paths are allowed",
            "error"
        );

        renderApiError(
            "Please enter a Vehicle Explorer API path beginning with /api/."
        );

        return;

    }

    sendApiButton.disabled = true;
    copyJsonButton.disabled = true;

    lastApiJson = null;

    setApiStatus(
        "Loading...",
        "loading"
    );

    apiResponse.textContent =
        "Loading...";

    try {

        const response =
            await fetch(url);

        const responseText =
            await response.text();

        let data;

        try {

            data =
                responseText
                    ? JSON.parse(responseText)
                    : null;

        }
        catch {

            data = responseText;

        }

        if (response.ok) {

            setApiStatus(
                `${response.status} ${response.statusText}`,
                "success"
            );

        }
        else {

            setApiStatus(
                `${response.status} ${response.statusText}`,
                "error"
            );

        }

        if (
            typeof data === "object" &&
            data !== null
        ) {

            lastApiJson =
                JSON.stringify(
                    data,
                    null,
                    2
                );

            renderJson(data);

            copyJsonButton.disabled = false;

        }
        else {

            lastApiJson =
                String(data ?? "");

            apiResponse.textContent =
                lastApiJson;

            copyJsonButton.disabled =
                lastApiJson.length === 0;

        }

    }
    catch (error) {

        setApiStatus(
            "Request failed",
            "error"
        );

        renderApiError(
            error.message
        );

    }
    finally {

        sendApiButton.disabled = false;

    }

}


// =====================================================
// JSON Rendering
// =====================================================

function renderJson(data) {

    const json =
        JSON.stringify(
            data,
            null,
            2
        );

    apiResponse.innerHTML =
        syntaxHighlightJson(json);

}


function syntaxHighlightJson(json) {

    /*
     * Escape HTML first because the final value
     * is assigned using innerHTML for highlighting.
     */
    const escaped =
        json
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;");

    return escaped.replace(
        /("(\\u[a-fA-F0-9]{4}|\\[^u]|[^\\"])*"\s*:)|("(\\u[a-fA-F0-9]{4}|\\[^u]|[^\\"])*")|\b(true|false)\b|\bnull\b|-?\d+(?:\.\d+)?(?:[eE][+-]?\d+)?/g,
        match => {

            let cssClass =
                "json-number";

            if (/^"/.test(match)) {

                if (/:$/.test(match)) {

                    cssClass =
                        "json-key";

                }
                else {

                    cssClass =
                        "json-string";

                }

            }
            else if (
                /true|false/.test(match)
            ) {

                cssClass =
                    "json-boolean";

            }
            else if (
                /null/.test(match)
            ) {

                cssClass =
                    "json-null";

            }

            return (
                `<span class="${cssClass}">` +
                `${match}` +
                `</span>`
            );

        }
    );

}


function renderApiError(text) {

    apiResponse.textContent =
        text;

}


// =====================================================
// API Status
// =====================================================

function setApiStatus(
    text,
    type
) {

    apiStatus.textContent =
        text;

    apiStatus.className =
        "status-badge";

    if (type) {

        apiStatus.classList.add(
            type
        );

    }

}


// =====================================================
// Copy JSON
// =====================================================

copyJsonButton.addEventListener(
    "click",
    async () => {

        if (!lastApiJson) {
            return;
        }

        try {

            await navigator.clipboard.writeText(
                lastApiJson
            );

            const originalText =
                copyJsonButton.textContent;

            copyJsonButton.textContent =
                "Copied!";

            setTimeout(
                () => {

                    copyJsonButton.textContent =
                        originalText;

                },
                1500
            );

        }
        catch {

            copyJsonButton.textContent =
                "Copy failed";

        }

    }
);


// =====================================================
// Clear Helpers
// =====================================================

function clearVehicleType() {

    vehicleTypeInput.value = "";

    vehicleTypeInput.disabled = true;

    vehicleTypes = [];

    vehicleTypeDropdown.innerHTML = "";

    vehicleTypeDropdown
        .classList
        .remove("open");

    updateClearButtons();

}


function updateClearButtons() {

    clearMakeButton
        .classList
        .toggle(
            "visible",
            makeInput.value.length > 0
        );

    clearVehicleTypeButton
        .classList
        .toggle(
            "visible",
            vehicleTypeInput.value.length > 0 &&
            !vehicleTypeInput.disabled
        );

}


// =====================================================
// Dropdown Helpers
// =====================================================

function filterByName(
    items,
    searchText
) {

    const search =
        normalize(searchText);

    if (!search) {
        return items;
    }

    return items.filter(
        item =>
            normalize(item.name)
                .includes(search)
    );

}


function normalize(value) {

    return (value ?? "")
        .trim()
        .toLowerCase();

}


function addEmptyItem(
    dropdown,
    text
) {

    const item =
        document.createElement("div");

    item.className =
        "dropdown-empty";

    item.textContent = text;

    dropdown.appendChild(item);

}


// =====================================================
// Close Dropdowns
// =====================================================

document.addEventListener(
    "click",
    event => {

        if (
            !event.target.closest(
                ".searchable-select"
            )
        ) {

            makeDropdown
                .classList
                .remove("open");

            vehicleTypeDropdown
                .classList
                .remove("open");

        }

    }
);


// =====================================================
// General Helpers
// =====================================================

function clearResults() {

    modelsContainer.innerHTML = "";

    message.textContent = "";

}


function showMessage(text) {

    message.textContent = text;

}
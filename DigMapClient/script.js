const API_BASE = "https://localhost:7231/api"; 

let token = localStorage.getItem("digmap_token");
let isLoginMode = true;
let editingId = null;

const map = L.map('map').setView([49.4, 32.0], 6);

L.tileLayer('https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}{r}.png', {
    attribution: '&copy; OpenStreetMap &copy; CARTO',
    subdomains: 'abcd',
    maxZoom: 20
}).addTo(map);

function checkAuth() {
    if (token) {
        document.getElementById("authScreen").classList.add("hidden");
        document.getElementById("mainContent").classList.remove("hidden");
        document.getElementById("btnAdd").classList.remove("hidden");
        document.getElementById("btnLogout").classList.remove("hidden");
        loadFinds(); 
        setTimeout(() => map.invalidateSize(), 100);
    } else {
        document.getElementById("authScreen").classList.remove("hidden");
        document.getElementById("mainContent").classList.add("hidden");
        document.getElementById("btnAdd").classList.add("hidden");
        document.getElementById("btnLogout").classList.add("hidden");
    }
}

function toggleAuthMode() {
    isLoginMode = !isLoginMode;
    document.getElementById("authTitle").innerText = isLoginMode ? "Вхід до системи" : "Створення профілю";
    document.getElementById("btnSubmit").innerText = isLoginMode ? "УВІЙТИ" : "ЗАРЕЄСТРУВАТИСЯ";
    document.getElementById("authSwitchText").innerText = isLoginMode ? "Ще не з нами? " : "Вже є акаунт? ";
    
    document.getElementById("authSwitchBtn").innerText = isLoginMode ? "Зареєструватися" : "Увійти";
    document.getElementById("authError").classList.add("hidden");
}

async function handleAuth(e) {
    e.preventDefault();

    const errDiv = document.getElementById("authError");
    errDiv.classList.add("hidden");
    errDiv.innerText = "";

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;
    const endpoint = isLoginMode ? "/auth/login" : "/auth/register";

    try {
        const res = await fetch(API_BASE + endpoint, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password })
        });

        const contentType = res.headers.get("content-type");
        let data;

        if (contentType && contentType.includes("application/json")) {
            data = await res.json();
        } else {
            data = await res.text();
        }

        if (!res.ok) {
            let errorMessage = "Сталася помилка авторизації";

            if (Array.isArray(data)) {
                errorMessage = data.map(err => {
                    let errorText = typeof err === 'string' ? err : (err.description || "Невідома помилка");

                    if (errorText.includes("DuplicateUserName") || errorText.includes("is already taken")) 
                        return "⚠️ Ця пошта вже зареєстрована!";
                    if (errorText.includes("PasswordTooShort") || errorText.includes("too short")) 
                        return "⚠️ Пароль надто короткий (мін. 4 символи).";
                    if (errorText.includes("Невірний логін або пароль")) 
                        return "⛔ Невірний логін або пароль. Спробуйте ще раз.";

                    return "⚠️ " + errorText;
                }).join("\n");
            }
            else if (typeof data === 'object' && data.message) {
                errorMessage = data.message;
            }
            else if (typeof data === 'string') {
                errorMessage = data.replace(/^"|"$/g, '');
                if (errorMessage.includes("Невірний логін або пароль")) {
                    errorMessage = "⛔ Невірний логін або пароль. Спробуйте ще раз.";
                }
            }

            errDiv.innerText = errorMessage;
            errDiv.classList.remove("hidden");
            return;
        }

        const tokenValue = data.token || (typeof data === 'object' ? data.token : null);

        if (tokenValue) {
            token = tokenValue;
            localStorage.setItem("digmap_token", token);
            
            if (!isLoginMode) {
                alert("🎉 Реєстрація успішна! Ласкаво просимо.");
            }
            checkAuth();
        }

    } catch (err) {
        console.error(err);
        errDiv.innerText = "🔌 Помилка з'єднання з сервером. Перевірте інтернет.";
        errDiv.classList.remove("hidden");
    }
}

function logout() {
    localStorage.removeItem("digmap_token");
    token = null;
    location.reload();
}

async function loadFinds() {
    try {
        const res = await fetch(API_BASE + "/finds", {
            method: "GET",
            headers: { 
                "Authorization": "Bearer " + token
            }
        });

        if (res.status === 401) { logout(); return; }

        const finds = await res.json();
        
        map.eachLayer(l => { if (l instanceof L.Marker) map.removeLayer(l); });
        const listDiv = document.getElementById("list");
        listDiv.innerHTML = "";

        finds.forEach(item => {
            L.marker([item.latitude, item.longitude]).addTo(map)
                .bindPopup(`<b>${item.name}</b><br>${item.description}`);

                const isCoin = item.$type === "coin";

                let badgeClass = isCoin ? "badge-coin" : "badge-artifact";
                let badgeText = isCoin ? "💰 Монета" : "🏺 Артефакт";
                let borderClass = isCoin ? "border-coin" : "border-artifact";
            
            listDiv.innerHTML += `
                <div class="col-md-4 col-sm-6">
                    <div class="card find-card h-100 ${borderClass}">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center mb-3">
                                <span class="badge rounded-pill ${badgeClass} px-3 py-2">${badgeText}</span>
                                <small class="text-muted fw-bold">${new Date(item.dateFound).toLocaleDateString()}</small>
                            </div>
                            <h5 class="card-title fw-bold text-dark">${item.name}</h5>
                            <p class="card-text text-secondary">${item.description || 'Опис відсутній...'}</p>
                        </div>
                        <div class="card-footer bg-white border-0 d-flex justify-content-end gap-2 pb-3 pe-3">
                            <button class="btn btn-sm btn-outline-primary rounded-pill px-3" onclick='startEdit(${JSON.stringify(item)})'>✏️ Ред.</button>
                            <button class="btn btn-sm btn-outline-danger rounded-pill px-3" onclick="deleteItem(${item.id})">🗑️ Вид.</button>
                        </div>
                    </div>
                </div>`;
        });
    } catch (e) { console.error(e); }
}

async function saveData() {
    const name = document.getElementById('inpName').value;
    const lat = document.getElementById('inpLat').value;
    const lng = document.getElementById('inpLng').value;
    const type = document.getElementById('itemType').value;

    if (!name.trim()) {
        alert("⚠️ Будь ласка, введіть назву знахідки!");
        return;
    }
    if (!lat || !lng) {
        alert("⚠️ Ви забули вказати місце на карті!\nКлікніть по карті або натисніть кнопку GPS.");
        return;
    }

    let data = {
        $type: type,
        id: editingId ? editingId : 0,
        name: name,
        description: document.getElementById('inpDesc').value,
        latitude: parseFloat(lat),
        longitude: parseFloat(lng),
        dateFound: new Date().toISOString()
    };

    if (type === 'coin') {
        data.year = parseInt(document.getElementById('inpYear').value) || 0;
        data.metal = document.getElementById('inpMetal').value;
        data.denomination = document.getElementById('inpDenom').value;
    } else {
        data.era = document.getElementById('inpEra').value;
        data.material = document.getElementById('inpMaterial').value;
        data.class = document.getElementById('inpClass').value;
    }

    const method = editingId ? "PUT" : "POST";
    const url = editingId ? `${API_BASE}/finds/${type}/${editingId}` : `${API_BASE}/finds/${type}`;

    try {
        const res = await fetch(url, {
            method: method,
            headers: { 
                "Content-Type": "application/json",
                "Authorization": "Bearer " + token 
            },
            body: JSON.stringify(data)
        });

        const contentType = res.headers.get("content-type");
        let responseData;
        if (contentType && contentType.includes("application/json")) {
            responseData = await res.json();
        } else {
            responseData = await res.text();
        }

        if (res.ok) {
            bootstrap.Modal.getInstance(document.getElementById('addModal')).hide();
            loadFinds();
        } else {
            let errorText = "Не вдалося зберегти дані.";

            if (responseData.errors) {
                const messages = Object.values(responseData.errors).flat();
                errorText = "⚠️ Перевірте дані:\n" + messages.join("\n");
            } 
            else if (responseData.message) {
                errorText = "⚠️ " + responseData.message;
            }
            else if (typeof responseData === 'string') {
                errorText = "⚠️ Помилка: " + responseData;
            }

            alert(errorText);
        }
    } catch (e) { 
        console.error(e);
        alert("🔌 Помилка з'єднання! Перевірте інтернет.");
    }
}

async function deleteItem(id) {
    if (!confirm("Видалити цей запис з реєстру?")) return;

    try {
        const res = await fetch(`${API_BASE}/finds/${id}`, {
            method: "DELETE",
            headers: { "Authorization": "Bearer " + token }
        });

        if (res.ok) {
            loadFinds();
        } else {
            alert("Помилка видалення!");
        }
    } catch (e) { console.error(e); }
}

let tempMarker = null;
let isPicking = false;
const modalEl = document.getElementById('addModal');
const modal = new bootstrap.Modal(modalEl);

function openAddModal() {
    editingId = null;
    document.getElementById('addForm').reset();
    document.querySelector('.modal-title').innerText = "Новий запис";
    toggleFields();
    modal.show();
}

function startEdit(item) {
    editingId = item.id;
    
    const currentType = item.$type;
    document.getElementById('itemType').value = currentType;
    
    toggleFields();

    document.getElementById('inpName').value = item.name;
    document.getElementById('inpDesc').value = item.description;
    document.getElementById('inpLat').value = item.latitude;
    document.getElementById('inpLng').value = item.longitude;

    if (item.$type === 'coin') {
        document.getElementById('inpYear').value = item.year;
        document.getElementById('inpMetal').value = item.metal;
        document.getElementById('inpDenom').value = item.denomination;
    } else {
        document.getElementById('inpEra').value = item.era;
        document.getElementById('inpMaterial').value = item.material;
        document.getElementById('inpClass').value = item.class;
    }

    document.querySelector('.modal-title').innerText = "Редагування запису";
    modal.show();
}

function toggleFields() {
    const type = document.getElementById('itemType').value;
    document.getElementById('coinFields').classList.toggle('hidden', type !== 'coin');
    document.getElementById('artifactFields').classList.toggle('hidden', type === 'coin');
}

function chooseOnMap() {
    modal.hide();
    isPicking = true;
    document.getElementById('map').style.cursor = "crosshair"; 
    document.getElementById('mapTip').classList.remove('hidden');
}

map.on('click', function(e) {
    if (!isPicking) return;
    if (tempMarker) map.removeLayer(tempMarker);
    tempMarker = L.marker(e.latlng).addTo(map);
    document.getElementById('inpLat').value = e.latlng.lat.toFixed(6);
    document.getElementById('inpLng').value = e.latlng.lng.toFixed(6);
    isPicking = false;
    document.getElementById('map').style.cursor = "";
    document.getElementById('mapTip').classList.add('hidden');
    modal.show();
});

function getGPS() {
    if (!navigator.geolocation) return alert("GPS недоступний");
    navigator.geolocation.getCurrentPosition(pos => {
        const {latitude: lat, longitude: lng} = pos.coords;
        map.setView([lat, lng], 15);
        if (tempMarker) map.removeLayer(tempMarker);
        tempMarker = L.marker([lat, lng]).addTo(map);
        
        document.getElementById('inpLat').value = lat.toFixed(6);
        document.getElementById('inpLng').value = lng.toFixed(6);
    });
}

checkAuth();
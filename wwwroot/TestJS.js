document.addEventListener("DOMContentLoaded", () => {
    const BASE_URL = "https://localhost:7127";
    const responseElement = document.getElementById("api-response");
    const adminLoginLink = document.getElementById("admin-login-link");
    const adminModal = document.getElementById("admin-modal-backdrop");
    const closeModalButton = document.getElementById("modal-close-button");
    const loginForm = document.getElementById("admin-login-form");
    const loginErrorMsg = document.getElementById("login-error-message");

    if (adminLoginLink) {
        adminLoginLink.addEventListener("click", e => {
            e.preventDefault(); 
            loginErrorMsg.textContent = ""; 
            document.getElementById("admin-username").value = ""; 
            document.getElementById("admin-password").value = "";
            adminModal.classList.add("modal-visible");
        });
    }

    if (closeModalButton) {
        closeModalButton.addEventListener("click", () => {
            adminModal.classList.remove("modal-visible");
        });
    }

    if (adminModal) {
        adminModal.addEventListener("click", e => {
            if (e.target === adminModal) {
                adminModal.classList.remove("modal-visible");
            }
        });
    }

    if (loginForm) {
        loginForm.addEventListener("submit", e => {
            e.preventDefault();
            loginErrorMsg.textContent = ""; 

            const username = document.getElementById("admin-username").value;
            const password = document.getElementById("admin-password").value;

            if (username === "admin" && password === "admin123") {
                window.location.href = "AdminAcW.html";
            } else {
                loginErrorMsg.textContent = "Invalid username or password.";
            }
        });
    }
    function showResponse(data, status) {
        responseElement.style.color = "#9fefc0";
        responseElement.textContent = `Status: ${status}\n\n${data ? JSON.stringify(data, null, 2) : "No content returned."}`;
    }

    async function showError(error, response) {
        let msg = `Error: ${error.message}\nStatus: ${response?.status || "N/A"}\n\n`;
        if (response) {
            try { msg += JSON.stringify(await response.json(), null, 2); }
            catch { msg += "Could not parse error response."; }
        }
        responseElement.style.color = "#e02424";
        responseElement.textContent = msg;
    }

    async function apiFetch(endpoint, method = 'GET', body = null) {
        console.log(`Fetching: ${method} ${endpoint}`);
        responseElement.style.color = "#00aaff";
        responseElement.textContent = "Loading...";

        const headers = {
            'Content-Type': 'application/json'
        };

        const options = {
            method: method,
            headers: headers
        };

        if (body) {
            options.body = JSON.stringify(body);
        }

        try {
            const res = await fetch(`${BASE_URL}${endpoint}`, options);
            console.log("Response status:", res.status);

            if (!res.ok) {
                return await showError(new Error(`HTTP ${res.status}`), res);
            }

            const data = res.status === 204 ? null : await res.json();
            showResponse(data, res.status);
        } catch (err) {
            console.error("Network error:", err);
            responseElement.style.color = "#e02424";
            responseElement.textContent = `Network Error: ${err.message}\nCheck API URL.`;
        }
    }

    document.getElementById("form-post-borrowing")?.addEventListener("submit", e => {
        e.preventDefault();

        const body = {
            BookISBN: document.getElementById("post-borrow-isbn").value,
            Name: document.getElementById("post-borrow-name").value,
            Year: parseInt(document.getElementById("post-borrow-year").value),
            Course: document.getElementById("post-borrow-course").value,
            Section: document.getElementById("post-borrow-section").value
        };

        apiFetch("/api/Borrowers", "POST", body);
    });

    const getForms = [
        { id: "form-get-books", endpoint: "/api/Books" },
        { id: "form-get-borrowers", endpoint: "/api/Borrowers" },
        { id: "form-get-returnees-borrowers", endpoint: "/api/Borrowers/returnees" },
        { id: "form-get-borrower", endpoint: id => `/api/Borrowers/${id}` },
        { id: "form-get-returnees", endpoint: "/api/ReturneeTbls" },
        { id: "form-get-returnee", endpoint: id => `/api/ReturneeTbls/${id}` }
    ];

    getForms.forEach(f => {
        const form = document.getElementById(f.id);
        if (!form) {
            console.warn(`Form with ID "${f.id}" not found.`);
            return;
        }
        form.addEventListener("submit", e => {
            e.preventDefault();
            const input = form.querySelector("input");
            const endpoint = typeof f.endpoint === "function"
                ? f.endpoint(input ? input.value : "")
                : f.endpoint;

            apiFetch(`${endpoint}`, "GET", null);
        });
    });
});
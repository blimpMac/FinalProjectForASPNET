document.addEventListener("DOMContentLoaded", () => {
    const BASE_URL = "https://finalproject.bsite.net/api";
    const responseBox = document.getElementById("api-response");
    const tableBody = document.querySelector("#books-table tbody");

    const showMessage = (msg, color = "#9fefc0") => {
        responseBox.style.color = color;
        responseBox.textContent = msg;
    };

    const safeGet = id => document.getElementById(id);

    // This function now correctly formats 400 validation errors
    const sendRequest = async (url, method, data = null, autoRefresh = false) => {
        showMessage(`Fetching: ${method} ${url}`, "#00aaff");
        try {
            const res = await fetch(url, {
                method,
                headers: { "Content-Type": "application/json" },
                body: data ? JSON.stringify(data) : null
            });

            let body;
            if (res.status === 204) {
                body = "Operation successful (No Content)";
            } else {
                const text = await res.text();
                try {
                    body = JSON.parse(text);
                } catch {
                    body = text || "No response body";
                }
            }

            console.log(`Response status: ${res.status}`);

            let message;
            if (res.status === 409)
                message = "⚠️ Conflict: Record may already exist or is in use.";
            else if (res.status === 404)
                message = "❌ Not Found: No matching record.";
            else if (res.status === 400 && body.errors) {
                // This now correctly parses the validation error
                message = "⚠️ Validation error:\n";
                for (const key in body.errors) {
                    message += `${key}: ${body.errors[key].join(", ")}\n`;
                }
            }
            else
                message = typeof body === "object" ? JSON.stringify(body, null, 2) : body;

            showMessage(`Status: ${res.status}\n\n${message}`, res.ok ? "#9fefc0" : "#e02424");

            if (res.ok && autoRefresh) loadBooks();
        } catch (err) {
            showMessage(`Network Error: ${err.message}`, "#e02424");
        }
    };

    const loadBooks = async () => {
        try {
            const res = await fetch(`${BASE_URL}/Books`);
            if (!res.ok) throw new Error(`HTTP ${res.status}`);
            const books = await res.json();
            tableBody.innerHTML = "";

            if (!books || !books.length) {
                const row = tableBody.insertRow();
                const cell = row.insertCell();
                cell.colSpan = 5;
                cell.textContent = "No books found.";
                return;
            }

            books.forEach(book => {
                const row = tableBody.insertRow();
                (row.insertCell()).textContent = book.BookISBN || "";
                (row.insertCell()).textContent = book.BookName || "";
                (row.insertCell()).textContent = book.AuthorName || "";
                (row.insertCell()).textContent = book.Course || "";
                (row.insertCell()).textContent = book.Section || "";
            });
        } catch (err) {
            showMessage(`Error loading books: ${err.message}`, "#e02424");
        }
    };

    safeGet("btn-get-books")?.addEventListener("click", loadBooks);

    safeGet("form-post-book")?.addEventListener("submit", e => {
        e.preventDefault();
        const bookISBN = safeGet("post-book-isbn")?.value.trim();
        const bookName = safeGet("post-book-name")?.value.trim();
        const authorName = safeGet("post-author-name")?.value.trim();
        // Read from the new fields
        const course = safeGet("post-course")?.value.trim();
        const section = safeGet("post-section")?.value.trim();

        if (!bookISBN || !bookName || !authorName || !course || !section) {
            showMessage("All fields are required!", "#e02424");
            return;
        }

        const book = {
            BookISBN: bookISBN,
            BookName: bookName,
            AuthorName: authorName,
            Course: course,
            Section: section
        };

        sendRequest(`${BASE_URL}/Books`, "POST", book, true);
        e.target.reset();
    });

    safeGet("form-put-book")?.addEventListener("submit", e => {
        e.preventDefault();
        const isbn = safeGet("put-book-isbn")?.value.trim();
        const bookName = safeGet("put-book-name")?.value.trim();
        const authorName = safeGet("put-book-author")?.value.trim();
        const course = safeGet("put-course")?.value.trim();
        const section = safeGet("put-section")?.value.trim();

        if (!isbn || !bookName || !authorName || !course || !section) {
            showMessage("All fields are required!", "#e02424");
            return;
        }

        const book = {
            BookISBN: isbn,
            BookName: bookName,
            AuthorName: authorName,
            Course: course,
            Example: "BSIT",
            Section: section
        };

        sendRequest(`${BASE_URL}/Books/${isbn}`, "PUT", book, true);
        e.target.reset();
    });

    safeGet("form-delete-book")?.addEventListener("submit", e => {
        e.preventDefault();
        const isbn = safeGet("delete-book-isbn")?.value.trim();
        if (!isbn) {
            showMessage("ISBN is required!", "#e02424");
            return;
        }
        sendRequest(`${BASE_URL}/Books/${isbn}`, "DELETE", null, true);
        e.target.reset();
    });

    safeGet("form-post-borrower")?.addEventListener("submit", e => {
        e.preventDefault();
        const name = safeGet("post-borrower-name")?.value.trim();
        const email = safeGet("post-borrower-email")?.value.trim();

        if (!name || !email) {
            showMessage("All fields are required!", "#e02424");
            return;
        }

        const borrower = { BorrowerName: name, BorrowerEmail: email };
        sendRequest(`${BASE_URL}/Borrowers`, "POST", borrower);
        e.target.reset();
    });

    loadBooks();
});
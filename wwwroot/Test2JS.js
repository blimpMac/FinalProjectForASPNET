document.addEventListener("DOMContentLoaded", () => {
    const BASE_URL = "https://localhost:7127";

    const ids = ["total-visitors", "total-borrowers", "borrowed-today", "overdue-books", "recent-borrowers"];
    const elements = Object.fromEntries(ids.map(id => [id, document.getElementById(id)]));

    const state = { chartData: null, analyticsChart: null, days: 30 };

    const chartToggle = document.getElementById("chart-days-toggle");
    if (chartToggle) {
        chartToggle.addEventListener("change", async e => {
            state.days = Number(e.target.value);
            await updateDashboard();
        });
    }

    async function fetchData(endpoint) {
        try {
            const res = await fetch(`${BASE_URL}${endpoint}`);
            return res.ok ? await res.json() : null;
        } catch (err) {
            console.error("Fetch error:", err);
            return null;
        }
    }

    function updateElement(id, text) {
        const el = elements[id];
        if (el && el.textContent !== text)
            requestAnimationFrame(() => el.textContent = text);
    }

    function updateRecentBorrowers(list) {
        const el = elements["recent-borrowers"];
        if (!el) return;
        const text = !list?.length
            ? "No recent activity or error fetching data."
            : list.map(b =>
                `${b.Name || "N/A"} - ${b.BookISBN || "N/A"} - ${b.BorrowedOn || "N/A"}`
            ).join("\n");
        updateElement("recent-borrowers", text);
    }

    function updateChart(newData) {
        const ctx = document.getElementById("analyticsChart")?.getContext("2d");
        if (!ctx) return;

        if (!newData?.labels?.length || !newData?.datasets?.length) {
            ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
            ctx.fillStyle = "#6c757d";
            ctx.textAlign = "center";
            ctx.fillText("Chart data unavailable", ctx.canvas.width / 2, ctx.canvas.height / 2);
            return;
        }

        if (JSON.stringify(state.chartData) === JSON.stringify(newData)) return;
        state.chartData = newData;

        if (state.analyticsChart) state.analyticsChart.destroy();

        state.analyticsChart = new Chart(ctx, {
            type: "line",
            data: newData,
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    title: {
                        display: true,
                        text: `Borrowing Trends (Last ${state.days} Days)`,
                        font: { size: 16 }
                    }
                },
                scales: {
                    x: { ticks: { color: "#666" }, grid: { display: false } },
                    y: {
                        beginAtZero: true,
                        ticks: { precision: 0 },
                        grid: { color: "rgba(200,200,200,0.2)" }
                    }
                },
                animation: false
            }
        });
    }

    async function updateDashboard() {
        const endpoints = [
            "/api/dashboard/visitors/monthly",
            "/api/dashboard/borrowers/active/count",
            "/api/dashboard/borrowers/today/count",
            "/api/dashboard/books/overdue/count",
            "/api/dashboard/borrowers/recent?limit=5",
            `/api/dashboard/trends?days=${state.days}`
        ];

        ids.forEach(id => updateElement(id, "Loading..."));

        const [visitors, borrowers, today, overdue, recent, chart] = await Promise.all(
            endpoints.map(fetchData)
        );

        updateElement("total-visitors", visitors?.count ?? "N/A");
        updateElement("total-borrowers", borrowers?.count ?? "N/A");
        updateElement("borrowed-today", today?.count ?? "N/A");
        updateElement("overdue-books", overdue?.count ?? "N/A");
        updateRecentBorrowers(recent);
        updateChart(chart);
    }

    updateDashboard();
    setInterval(updateDashboard, 60000);
});

const API_BASE_URL = "http://localhost:5166";

export async function getDashboardSummary() {
    const response = await fetch(`${API_BASE_URL}/api/dashboard/summary`);

    if (!response.ok) {
        throw new Error("Failed to load dashboard summary");
    }

    return response.json();
}

export async function getClients() {
    const response = await fetch(`${API_BASE_URL}/api/clients`);

    if (!response.ok) {
        throw new Error("Failed to load clients");
    }

    return response.json();
}

export async function getProjects() {
    const response = await fetch(`${API_BASE_URL}/api/projects`);

    if (!response.ok) {
        throw new Error("Failed to load projects");
    }

    return response.json();
}
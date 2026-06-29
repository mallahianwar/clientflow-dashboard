import { useEffect, useState } from "react";
import { getDashboardSummary } from "../api/clientApi";

type DashboardSummary = {
    totalClients: number;
    activeClients: number;
    totalProjects: number;
    plannedProjects: number;
    inProgressProjects: number;
    completedProjects: number;
    totalBudget: number;
};

export default function DashboardPage() {
    const [summary, setSummary] = useState<DashboardSummary | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        getDashboardSummary()
            .then((data) => setSummary(data))
            .catch(() => setError("Could not load dashboard data."))
            .finally(() => setLoading(false));
    }, []);

    if (loading) return <p>Loading dashboard...</p>;
    if (error) return <p>{error}</p>;
    if (!summary) return <p>No data found.</p>;

    return (
        <div>
            <h1>ClientFlow Dashboard</h1>

            <div style={{ display: "grid", gridTemplateColumns: "repeat(3, 1fr)", gap: "16px" }}>
                <div>
                    <h3>Total Clients</h3>
                    <p>{summary.totalClients}</p>
                </div>

                <div>
                    <h3>Active Clients</h3>
                    <p>{summary.activeClients}</p>
                </div>

                <div>
                    <h3>Total Projects</h3>
                    <p>{summary.totalProjects}</p>
                </div>

                <div>
                    <h3>Planned</h3>
                    <p>{summary.plannedProjects}</p>
                </div>

                <div>
                    <h3>In Progress</h3>
                    <p>{summary.inProgressProjects}</p>
                </div>

                <div>
                    <h3>Completed</h3>
                    <p>{summary.completedProjects}</p>
                </div>
            </div>
        </div>
    );
}
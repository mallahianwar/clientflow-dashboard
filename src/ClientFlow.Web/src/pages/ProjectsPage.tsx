import { useEffect, useState } from "react";
import { getProjects } from "../api/clientApi";

type Project = {
    id: number;
    clientId: number;
    clientName: string;
    name: string;
    description?: string;
    status: string;
    budget?: number;
};

export default function ProjectsPage() {
    const [projects, setProjects] = useState<Project[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        getProjects()
            .then((result) => setProjects(result.data))
            .finally(() => setLoading(false));
    }, []);

    if (loading) return <p>Loading projects...</p>;

    return (
        <div>
            <h1>Projects</h1>

            <table border={1} cellPadding={8}>
                <thead>
                    <tr>
                        <th>Project</th>
                        <th>Client</th>
                        <th>Status</th>
                        <th>Budget</th>
                    </tr>
                </thead>

                <tbody>
                    {projects.map((project) => (
                        <tr key={project.id}>
                            <td>{project.name}</td>
                            <td>{project.clientName}</td>
                            <td>{project.status}</td>
                            <td>{project.budget}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
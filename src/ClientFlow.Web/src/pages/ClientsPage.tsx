import { useEffect, useState } from "react";
import { getClients } from "../api/clientApi";

type Client = {
    id: number;
    name: string;
    email: string;
    companyName?: string;
    phoneNumber?: string;
    status: string;
    projectCount: number;
};

export default function ClientsPage() {
    const [clients, setClients] = useState<Client[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        getClients()
            .then((result) => setClients(result.data))
            .finally(() => setLoading(false));
    }, []);

    if (loading) return <p>Loading clients...</p>;

    return (
        <div>
            <h1>Clients</h1>

            <table border={1} cellPadding={8}>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Company</th>
                        <th>Status</th>
                        <th>Projects</th>
                    </tr>
                </thead>

                <tbody>
                    {clients.map((client) => (
                        <tr key={client.id}>
                            <td>{client.name}</td>
                            <td>{client.email}</td>
                            <td>{client.companyName}</td>
                            <td>{client.status}</td>
                            <td>{client.projectCount}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
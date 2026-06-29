import { BrowserRouter, Link, Route, Routes } from "react-router-dom";
import DashboardPage from "./pages/DashboardPage";
import ClientsPage from "./pages/ClientsPage";
import ProjectsPage from "./pages/ProjectsPage";

function App() {
  return (
    <BrowserRouter>
      <div style={{ display: "flex", minHeight: "100vh" }}>
        <aside style={{ width: "220px", padding: "20px", borderRight: "1px solid #ddd" }}>
          <h2>ClientFlow</h2>
          <nav style={{ display: "flex", flexDirection: "column", gap: "12px" }}>
            <Link to="/">Dashboard</Link>
            <Link to="/clients">Clients</Link>
            <Link to="/projects">Projects</Link>
          </nav>
        </aside>

        <main style={{ flex: 1, padding: "24px" }}>
          <Routes>
            <Route path="/" element={<DashboardPage />} />
            <Route path="/clients" element={<ClientsPage />} />
            <Route path="/projects" element={<ProjectsPage />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}

export default App;
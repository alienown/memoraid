import { Outlet } from "react-router-dom";
import { Navbar } from "../components/Navbar";

export function RootLayout() {
  return (
    <div className="flex flex-col h-full">
      <Navbar />
      <main className="py-6 h-full">
        <Outlet />
      </main>
    </div>
  );
}

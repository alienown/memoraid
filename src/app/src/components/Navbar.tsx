import { Link, useLocation, useNavigate } from "react-router-dom";
import {
  NavigationMenu,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
} from "@/components/ui/navigation-menu";
import { cn } from "@/lib/utils";
import { useAuth } from "@/core/auth/useAuth";

export function Navbar() {
  const location = useLocation();
  const navigate = useNavigate();
  const { isAuthenticated, logout } = useAuth();

  const isActive = (path: string) => {
    return location.pathname === path;
  };

  const handleLogout = async () => {
    await logout();
    navigate("/login");
  };

  return (
    <div className="sticky w-full top-0 bg-gray-200 z-10 px-5 rounded-xl">
      <div className="flex h-14 items-center">
        <Link to="/">
          <div className="mr-4 text-xl">Memoraid</div>
        </Link>
        {isAuthenticated && (
          <NavigationMenu>
            <NavigationMenuList>
              <NavigationMenuItem>
                <NavigationMenuLink
                  className={cn(
                    isActive("/generate") &&
                      "bg-accent text-accent-foreground"
                  )}
                  asChild
                >
                  <Link to="/generate">Generate </Link>
                </NavigationMenuLink>
              </NavigationMenuItem>
              <NavigationMenuItem>
                <NavigationMenuLink
                  className={cn(
                    isActive("/flashcards") &&
                      "bg-accent text-accent-foreground"
                  )}
                  asChild
                >
                  <Link to="/flashcards">My flashcards</Link>
                </NavigationMenuLink>
              </NavigationMenuItem>
            </NavigationMenuList>
          </NavigationMenu>
        )}
        <div className="ml-auto flex gap-2">
          <NavigationMenu>
            <NavigationMenuList>
              {!isAuthenticated && (
                <>
                  <NavigationMenuItem>
                    <NavigationMenuLink
                      className={cn(
                        isActive("/login") &&
                          "bg-accent text-accent-foreground"
                      )}
                      asChild
                    >
                      <Link to="/login">Login</Link>
                    </NavigationMenuLink>
                  </NavigationMenuItem>
                  <NavigationMenuItem>
                    <NavigationMenuLink
                      className={cn(
                        isActive("/register") &&
                          "bg-accent text-accent-foreground"
                      )}
                      asChild
                    >
                      <Link to="/register">Register</Link>
                    </NavigationMenuLink>
                  </NavigationMenuItem>
                </>
              )}
              {isAuthenticated && (
                <NavigationMenuItem>
                  <NavigationMenuLink
                    onClick={handleLogout}
                    className={cn(
                      isActive("/logout") &&
                        "bg-accent text-accent-foreground"
                    )}
                  >
                    Logout
                  </NavigationMenuLink>
                </NavigationMenuItem>
              )}
            </NavigationMenuList>
          </NavigationMenu>
        </div>
      </div>
    </div>
  );
}

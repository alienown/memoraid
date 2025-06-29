import { Link, useLocation } from "react-router-dom";
import { useState, useEffect } from "react";
import {
  NavigationMenu,
  NavigationMenuList,
} from "@/components/ui/navigation-menu";
import { Sheet, SheetContent, SheetTrigger } from "@/components/ui/sheet";
import { Button } from "@/components/ui/button";
import { Menu } from "lucide-react";
import { useAuth } from "@/core/auth/useAuth";
import { useMobile } from "@/hooks";
import { NavbarLink } from "@/components/NavbarLink";

export function Navbar() {
  const location = useLocation();
  const { isAuthenticated, logout } = useAuth();
  const isMobile = useMobile();
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState<boolean>(false);

  useEffect(() => {
    if (!isMobile && isMobileMenuOpen) {
      setIsMobileMenuOpen(false);
    }
  }, [isMobile, isMobileMenuOpen]);

  const isActive = (path: string) => {
    return location.pathname === path;
  };

  const handleLogout = async () => {
    setIsMobileMenuOpen(false);
    
    await logout();
  };

  const handleLinkClick = () => {
    setIsMobileMenuOpen(false);
  };

  const NavItems = () => (
    <>
      {isAuthenticated && (
        <>
          <NavbarLink
            to="/generate"
            isActive={isActive("/generate")}
            onClick={handleLinkClick}
          >
            Generate
          </NavbarLink>
          <NavbarLink
            to="/flashcards"
            isActive={isActive("/flashcards")}
            onClick={handleLinkClick}
          >
            My flashcards
          </NavbarLink>
        </>
      )}
    </>
  );

  const AuthItems = () => (
    <>
      {!isAuthenticated ? (
        <>
          <NavbarLink
            to="/login"
            isActive={isActive("/login")}
            onClick={handleLinkClick}
          >
            Login
          </NavbarLink>
          <NavbarLink
            to="/register"
            isActive={isActive("/register")}
            onClick={handleLinkClick}
          >
            Register
          </NavbarLink>
        </>
      ) : (
        <NavbarLink to="/login" onClick={handleLogout}>
          Logout
        </NavbarLink>
      )}
    </>
  );

  return (
    <div className="sticky w-full top-0 bg-gray-200 z-10 px-5 rounded-xl">
      <div className="flex h-14 items-center">
        <Link to="/" onClick={handleLinkClick}>
          <div className="mr-4 text-xl">Memoraid</div>{" "}
        </Link>
        {!isMobile && (
          <div className="flex flex-grow">
            <NavigationMenu>
              <NavigationMenuList>
                <NavItems />
              </NavigationMenuList>
            </NavigationMenu>
            <div className="ml-auto">
              <NavigationMenu>
                <NavigationMenuList>
                  <AuthItems />
                </NavigationMenuList>
              </NavigationMenu>
            </div>
          </div>
        )}

        {isMobile && (
          <div className="ml-auto">
            <Sheet open={isMobileMenuOpen} onOpenChange={setIsMobileMenuOpen}>
              <SheetTrigger asChild>
                <Button variant="ghost" size="icon" className="h-9 w-9 p-0">
                  <Menu className="h-5 w-5" />
                  <span className="sr-only">Toggle menu</span>
                </Button>
              </SheetTrigger>
              <SheetContent side="right">
                <div className="flex flex-col space-y-4 mt-8">
                  <NavigationMenu className="max-w-full *:w-full mt-3">
                    <NavigationMenuList className="flex-col items-start space-y-3 gap-0">
                      <NavItems />
                      <AuthItems />
                    </NavigationMenuList>
                  </NavigationMenu>
                </div>
              </SheetContent>
            </Sheet>
          </div>
        )}
      </div>
    </div>
  );
}

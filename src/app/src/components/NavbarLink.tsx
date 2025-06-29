import { Link } from "react-router-dom";
import { cn } from "@/lib/utils";
import { NavigationMenuItem, NavigationMenuLink } from "@/components/ui/navigation-menu";

type NavbarLinkProps = {
  to: string;
  isActive?: boolean;
  onClick?: () => void;
  children: React.ReactNode;
};

export function NavbarLink({ to, isActive, onClick, children }: NavbarLinkProps) {
  const navMenuItemClasses = cn(
    // Mobile
    "w-full",
    "text-center",
    "border-b-1",
    "first:border-t-1",
    "m-0",
    // Desktop
    "sm:w-auto",
    "sm:text-left",
    "sm:border-b-0",
    "sm:border-t-0",
    "sm:m-1"
  );

  const linkClasses = cn(
    "p-3",
    "sm:p-2",
    isActive && "bg-accent text-accent-foreground"
  );

  return (
    <NavigationMenuItem className={navMenuItemClasses}>
      <NavigationMenuLink className={isActive ? "bg-accent text-accent-foreground" : ""} asChild>
        <Link to={to} onClick={onClick} className={linkClasses}>
          {children}
        </Link>
      </NavigationMenuLink>
    </NavigationMenuItem>
  );
}

import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
} from "@/components/ui/card";
import { cn } from "@/lib/utils";
import { CheckCircle, XCircle, Pencil, Eye, EyeOff } from "lucide-react";

export interface FlashcardListItemProps {
  front: string;
  back: string;
  isAccepted: boolean;
  onAccept: () => void;
  onReject: () => void;
  onEdit: () => void;
}

export function FlashcardListItem({
  front,
  back,
  isAccepted,
  onAccept,
  onReject,
  onEdit,
}: FlashcardListItemProps) {
  const [isBackVisible, setIsBackVisible] = useState(false);

  const toggleBackVisibility = () => {
    setIsBackVisible(!isBackVisible);
  };

  // Card styling based on acceptance state
  const cardClassName = cn(
    isAccepted
      ? "border-green-500 bg-green-50 dark:bg-green-950/30 dark:border-green-800"
      : "hover:border-gray-300"
  );

  return (
    <Card className={cardClassName}>
      <CardHeader className="flex flex-row items-start justify-between pb-2">
        <div className="ml-auto flex space-x-1">
          <Button
            onClick={onAccept}
            disabled={isAccepted}
            variant="ghost"
            size="icon"
            className="h-8 w-8"
            title="Accept"
          >
            <CheckCircle className="h-4 w-4" />
          </Button>
          <Button
            onClick={onReject}
            disabled={!isAccepted}
            variant="ghost"
            size="icon"
            className="h-8 w-8"
            title="Reject"
          >
            <XCircle className="h-4 w-4" />
          </Button>
          <Button
            onClick={onEdit}
            variant="ghost"
            size="icon"
            className="h-8 w-8"
            title="Edit"
          >
            <Pencil className="h-4 w-4" />
          </Button>
          <Button
            variant="ghost"
            size="icon"
            onClick={toggleBackVisibility}
            className="h-8 w-8"
            title={isBackVisible ? "Hide Answer" : "Show Answer"}
          >
            {isBackVisible ? (
              <EyeOff className="h-4 w-4" />
            ) : (
              <Eye className="h-4 w-4" />
            )}
          </Button>
        </div>
      </CardHeader>

      <CardContent className="space-y-4 flex-grow">
        {!isBackVisible ? (
          <div>
            <div className="font-medium">Front:</div>
            <p className="mt-1 break-words">{front}</p>
          </div>
        ) : (
          <div>
            <div className="font-medium">Back:</div>
            <p className="mt-1 break-words">{back}</p>
          </div>
        )}
      </CardContent>

      <CardFooter></CardFooter>
    </Card>
  );
}

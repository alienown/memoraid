import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
} from "@/components/ui/card";
import { Eye, EyeOff, Pencil, Trash2 } from "lucide-react";

export interface FlashcardListItemProps {
  id: number;
  front: string;
  back: string;
  onEdit: () => void;
  onDelete: () => void;
}

export function FlashcardListItem({
  front,
  back,
  onEdit,
  onDelete,
}: FlashcardListItemProps) {
  const [isBackVisible, setIsBackVisible] = useState(false);

  const toggleBackVisibility = () => {
    setIsBackVisible(!isBackVisible);
  };

  return (
    <Card
      className={"hover:border-gray-300"}
      data-testid="flashcard-item"
    >
      <CardHeader className="flex flex-row items-start justify-between pb-2">
        <div className="ml-auto flex space-x-1">
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
            onClick={onDelete}
            variant="ghost"
            size="icon"
            className="h-8 w-8"
            title="Delete"
          >
            <Trash2 className="h-4 w-4" />
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

      <CardContent className="space-y-4">
        {!isBackVisible && (
          <div>
            <p className="font-medium">Front:</p>
            <p className="mt-1 break-words" data-testid="flashcard-front-text">
              {front}
            </p>
          </div>
        )}

        {isBackVisible && (
          <div>
            <p className="font-medium">Back:</p>
            <p className="mt-1 break-words" data-testid="flashcard-back-text">
              {back}
            </p>
          </div>
        )}
      </CardContent>

      <CardFooter></CardFooter>
    </Card>
  );
}

export function FlashcardListItemSkeleton() {
  return (
    <Card className="animate-pulse">
      <CardHeader className="flex flex-row items-start justify-between pb-2">
        <div className="ml-auto flex space-x-1">
          <Button variant="ghost" size="icon" className="h-8 w-8" disabled>
            <Pencil className="h-4 w-4" />
          </Button>
          <Button variant="ghost" size="icon" className="h-8 w-8" disabled>
            <Trash2 className="h-4 w-4" />
          </Button>
          <Button variant="ghost" size="icon" className="h-8 w-8" disabled>
            <Eye className="h-4 w-4" />
          </Button>
        </div>
      </CardHeader>

      <CardContent className="space-y-4">
        <div>
          <p className="font-medium">Front:</p>
          <p className="h-4 w-2/3 mt-3 bg-gray-200 rounded"></p>
        </div>
      </CardContent>

      <CardFooter></CardFooter>
    </Card>
  );
}

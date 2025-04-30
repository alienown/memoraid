import { useState } from "react";
import { Button } from "@/components/ui/button";
import { FlashcardSource } from "../../api/api";
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
} from "@/components/ui/card";

export interface FlashcardListItemProps {
  front: string;
  back: string;
  source: FlashcardSource;
  isAccepted: boolean;
  onAccept: () => void;
  onReject: () => void;
  onEdit: () => void;
}

export function FlashcardListItem({
  front,
  back,
  source,
  isAccepted,
  onAccept,
  onReject,
  onEdit,
}: FlashcardListItemProps) {
  const [isBackVisible, setIsBackVisible] = useState(false);

  const toggleBackVisibility = () => {
    setIsBackVisible(!isBackVisible);
  };

  return (
    <Card>
      <CardHeader className="flex flex-row items-start justify-between pb-2">
        <div className="font-medium text-sm text-muted-foreground">
          {source === FlashcardSource.AIFull
            ? "AI Generated"
            : source === FlashcardSource.AIEdited
            ? "AI Generated (Edited)"
            : "Manual"}
        </div>
        <Button
          variant="ghost"
          size="sm"
          onClick={toggleBackVisibility}
          className="text-xs"
        >
          {isBackVisible ? "Hide Answer" : "Show Answer"}
        </Button>
      </CardHeader>

      <CardContent className="space-y-4">
        {!isBackVisible && (
          <div>
            <div className="font-medium">Front:</div>
            <p className="mt-1">{front}</p>
          </div>
        )}

        {isBackVisible && (
          <div>
            <div className="font-medium">Back:</div>
            <p className="mt-1">{back}</p>
          </div>
        )}
      </CardContent>

      <CardFooter className="flex space-x-2">
        <Button
          onClick={onAccept}
          disabled={isAccepted}
          variant="default"
          size="sm"
        >
          {isAccepted ? "Accepted" : "Accept"}
        </Button>
        <Button onClick={onReject} variant="destructive" size="sm">
          Reject
        </Button>
        <Button onClick={onEdit} variant="outline" size="sm">
          Edit
        </Button>
      </CardFooter>
    </Card>
  );
}

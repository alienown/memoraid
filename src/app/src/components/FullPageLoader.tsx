import { useEffect, useState } from "react";

interface FullPageLoaderProps {
  delay?: number;
}

export function FullPageLoader({ delay = 0 }: FullPageLoaderProps) {
  const [showLoader, setShowLoader] = useState(delay === 0);

  useEffect(() => {
    if (delay === 0) return;

    const timer = setTimeout(() => {
      setShowLoader(true);
    }, delay);

    return () => clearTimeout(timer);
  }, [delay]);

  return (
    <div className="absolute top-0 left-0 w-full h-full flex items-center justify-center bg-white">
      <div
        className={`flex items-center transition-opacity duration-300 ease-in-out opacity-0 ${
          showLoader ? "opacity-100" : ""
        }`}
      >
        <span className="text-2xl mr-3">Loading</span>
        <div
          className="animate-spin inline-block size-5 border-3 border-current border-t-transparent rounded-full"
          role="status"
          aria-label="loading"
        >
          <span className="sr-only">Loading...</span>
        </div>
      </div>
    </div>
  );
}

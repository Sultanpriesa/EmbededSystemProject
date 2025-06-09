import React, { useState, useEffect } from "react";

export default function Mailbox() {
  const [isOn, setIsOn] = useState(false);
  const [messages, setMessages] = useState([]);
  const BASEURL = import.meta.env.VITE_BASEURL;

  // Fetch the latest mail from backend
  const fetchLatestMail = async () => {
    try {
      const response = await fetch(`${BASEURL}/api/Movement/latest`);
      if (response.ok) {
        const data = await response.json();
        setMessages((prev) => {
          // If the new message is "on" and there's already an "on" message, do not add it
          if (
            data.message?.toLowerCase() === "on" &&
            prev.some((msg) => msg.message?.toLowerCase() === "on")
          ) {
            return prev;
          }
          // Avoid duplicates by moveID and keep only top 8 messages
          if (prev.length === 0 || prev[0].moveID !== data.moveID) {
            const msgWithTime = {
              ...data,
              timestamp: data.timestamp || new Date().toISOString(),
            };
            return [msgWithTime, ...prev].slice(0, 8);
          }
          return prev;
        });
      }
    } catch (error) {
      console.error("Failed to fetch latest mail:", error);
    }
  };

  useEffect(() => {
    fetchLatestMail();
    const interval = setInterval(fetchLatestMail, 5000);
    return () => clearInterval(interval);
  }, []);

  const handleButtonClick = async (newState) => {
    setIsOn(newState);
    const info = {
      message: newState ? "enable" : "disable",
      buttonSignal: 0,
      sensorSignal: 0,
      DataFrom: "Frontend",
    };
    try {
      await fetch(`${BASEURL}/api/Movement`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(info),
      });
      fetchLatestMail();
    } catch (error) {
      console.error("Failed to send movement:", error);
    }
  };

  return (
    <div className="w-screen h-screen bg-gray-100 flex flex-col items-center justify-start p-4 md:p-10">
      <div className="bg-white w-full h-full rounded-lg shadow-lg p-4 md:p-10">
        <h1 className="text-center text-lg md:text-2xl lg:text-3xl font-semibold border-b pb-4">
          YOUR MAILBOX 299 TWEED STREET
        </h1>

        <div className="flex justify-center space-x-6 md:space-x-10 mt-10">
          <button
            onClick={() => handleButtonClick(true)}
            className={`px-10 md:px-14 py-5 md:py-6 text-lg md:text-2xl rounded-full font-bold shadow transition-all duration-300 ${
              isOn ? "bg-green-400 text-white" : "bg-white border border-black"
            }`}
          >
            ON
          </button>

          <button
            onClick={() => handleButtonClick(false)}
            className={`px-10 md:px-14 py-5 md:py-6 text-lg md:text-2xl rounded-full font-bold shadow transition-all duration-300 ${
              !isOn ? "bg-red-400 text-white" : "bg-white border border-black"
            }`}
          >
            OFF
          </button>
        </div>

        <div className="mt-12 border-t pt-4 text-sm md:text-base lg:text-lg">
          <h2 className="font-semibold mb-2">Messages</h2>
          <div>
            {messages.length === 0 ? (
              <span className="text-gray-500">No messages yet.</span>
            ) : (
              messages.map((msg) => (
                <div
                  key={msg.moveID}
                  className="flex justify-between border-b py-1"
                >
                  <span className="text-blue-700 font-medium">{msg.message}</span>
                  <span className="text-right">
                    {msg.timestamp
                      ? new Date(msg.timestamp).toLocaleTimeString()
                      : ""}
                  </span>
                </div>
              ))
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

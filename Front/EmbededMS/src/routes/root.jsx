import React, { useState } from 'react';

export default function Mailbox() {
  const [isOn, setIsOn] = useState(false);

  return (
    <div className="w-screen h-screen bg-gray-100 flex flex-col items-center justify-start p-4 md:p-10">
      <div className="bg-white w-full h-full rounded-lg shadow-lg p-4 md:p-10">
        <h1 className="text-center text-lg md:text-2xl lg:text-3xl font-semibold border-b pb-4">YOUR MAILBOX 299 TWEED STREET</h1>

        <div className="flex justify-center space-x-6 md:space-x-10 mt-10">
          <button
            onClick={() => setIsOn(true)}
            className={`px-10 md:px-14 py-5 md:py-6 text-lg md:text-2xl rounded-full font-bold shadow transition-all duration-300 ${isOn ? 'bg-green-400 text-white' : 'bg-white border border-black'}`}
          >
            ON
          </button>

          <button
            onClick={() => setIsOn(false)}
            className={`px-10 md:px-14 py-5 md:py-6 text-lg md:text-2xl rounded-full font-bold shadow transition-all duration-300 ${!isOn ? 'bg-red-400 text-white' : 'bg-white border border-black'}`}
          >
            OFF
          </button>
        </div>

        <div className="mt-12 border-t pt-4 text-sm md:text-base lg:text-lg">
          <h2 className="font-semibold mb-2">Messages</h2>
          <div className="flex justify-between">
            <span className="text-blue-700 font-medium">You Got Mail!</span>
            <span className="text-right">10:00 AM</span>
          </div>
        </div>
      </div>
    </div>
  );
}

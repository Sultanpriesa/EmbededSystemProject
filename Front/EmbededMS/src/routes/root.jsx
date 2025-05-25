import React, { useState } from 'react';
export default function Root() {
    const [isOn, setIsOn] = useState(false);


    return (

        <div className="min-h-screen bg-gray-100 flex flex-col items-center justify-start pt-10">
            <div className="bg-white w-full max-w-3xl rounded-lg shadow-lg p-6">
                <h1 className="text-center text-xl font-semibold border-b pb-2">YOUR MAILBOX 261 TWEED STREET</h1>

                <div className="flex justify-center space-x-10 mt-10">
                    <button
                        onClick={() => setIsOn(true)}
                        className={`px-8 py-4 rounded-full text-lg font-bold shadow ${isOn ? 'bg-green-400 text-white' : 'bg-white border'}`}
                    >
                        ON
                    </button>

                    <button
                        onClick={() => setIsOn(false)}
                        className={`px-8 py-4 rounded-full text-lg font-bold shadow ${!isOn ? 'bg-red-400 text-white' : 'bg-white border'}`}
                    >
                        OFF
                    </button>
                </div>

                <div className="mt-12 border-t pt-4">
                    <h2 className="font-semibold mb-2">Messages</h2>
                    <div className="flex justify-between">
                        <span>You Got Mail</span>
                        <span>10:00 AM</span>
                    </div>
                </div>
            </div>
        </div>
    );

} 
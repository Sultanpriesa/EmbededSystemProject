using System;
using EMSAPI.Models;

namespace EMSAPI.Dtos;

public class MovementDto
{
    public string? Message { get; set; }
    public int ButtonSignal { get; set; }
    public int SensorSignal { get; set; }
    public string DataFrom {get; set;} = default!;

    public Movement ToMovementModel()
    {
        return new Movement
        {
            Message = this.Message,
            ButtonSignal = this.ButtonSignal,
            SensorSignal = this.SensorSignal,
            DataFrom = this.DataFrom
        };
    }
}

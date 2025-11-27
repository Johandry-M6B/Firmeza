namespace Domain.Enums;

public enum MovementType
{
    Input = 1, // Purchase from supplier
    Output = 2, // Sale
    Adjustment = 3, // Manual adjustment
    Return = 4, // Return from customer
    Decrease = 5, // Decrease due to damage or loss
    Transfer = 6 // Transfer between locations
}
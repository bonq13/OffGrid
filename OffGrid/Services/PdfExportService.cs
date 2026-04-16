using OffGrid.Models;
using OffGrid.UI;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace OffGrid.Services;

public class PdfExportService
{
    public void GenerateExpeditionReport(ExpeditionPlan expedition, string outputPath)
    {
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11));

                
                page.Header().Column(col =>
                {
                    col.Item().Text($"Plan Wyprawy: {expedition.Name}")
                        .FontSize(20).Bold().AlignCenter();
                    col.Item().Text($"Wygenerowano: {DateTime.Now:dd.MM.yyyy}")
                        .FontSize(9).AlignCenter().FontColor(Colors.Grey.Medium);
                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
                });
                
                page.Content().Column(col =>
                {
                    col.Spacing(10);
                    
                    col.Item().Text("Szczegóły wyprawy").FontSize(14).Bold();
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(1);
                            cols.RelativeColumn(2);
                        });

                        void AddRow(string label, string value)
                        {
                            table.Cell().Text(label).Bold();
                            table.Cell().Text(value);
                        }

                        AddRow("Data rozpoczęcia:", expedition.StartDate.ToString("dd.MM.yyyy"));
                        AddRow("Szacowana data zakończenia:", expedition.EndDate.ToString("dd.MM.yyyy"));
                        AddRow("Czas trwania:", $"{expedition.DurationDays} dni");
                        AddRow("Szacowana długość trasy:", $"{expedition.EstimatedKilometers} km");
                        AddRow("Liczba uczestników:", expedition.ExpeditionParticipants.ToString());
                        AddRow("Szacowany koszt:", $"{expedition.EstimatedCost:F2} zł");
                    });
                    
                    col.Item().Text("Punkty trasy").FontSize(14).Bold();
                    if (expedition.RoutePoints.Count == 0)
                    {
                        col.Item().Text("Brak zdefiniowanych punktów trasy.")
                            .FontColor(Colors.Grey.Medium);
                    }
                    else
                    {
                        foreach (var point in expedition.RoutePoints)
                        {
                            col.Item().Row(row =>
                            {
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text($"• {point.Name}").Bold();
                                    if (!string.IsNullOrEmpty(point.Notes))
                                        c.Item().Text($"  {point.Notes}")
                                            .FontColor(Colors.Grey.Darken2);
                                });
                            });
                        }
                    }
                    
                    col.Item().Text("Lista sprzętu").FontSize(14).Bold();
                    if (expedition.Gear == null || expedition.Gear.Items.Count == 0)
                    {
                        col.Item().Text("Brak sprzętu w plecaku.")
                            .FontColor(Colors.Grey.Medium);
                    }
                    else
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3);
                                cols.RelativeColumn(1);
                                cols.RelativeColumn(1);
                            });
                            
                            table.Header(header =>
                            {
                                header.Cell().Text("Przedmiot").Bold();
                                header.Cell().Text("Kategoria").Bold();
                                header.Cell().Text("Waga").Bold();
                            });

                            foreach (var item in expedition.Gear.Items)
                            {
                                table.Cell().Text(item.Name);
                                table.Cell().Text(item.Category.ToPolish());
                                table.Cell().Text($"{item.WeightGrams}g");
                            }
                        });

                        col.Item().Text(
                            $"Łączna waga plecaka: {expedition.Gear.CurrentWeightKg:F2} kg / " +
                            $"{expedition.Gear.MaxWeightKg} kg max")
                            .Bold();
                    }

                    
                    if (!string.IsNullOrEmpty(expedition.Notes))
                    {
                        col.Item().Text("Uwagi i notatki do wyprawy").FontSize(14).Bold();
                        col.Item().Text(expedition.Notes);
                    }
                });
                
                page.Footer().AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Strona ");
                        x.CurrentPageNumber();
                        x.Span(" z ");
                        x.TotalPages();
                    });
            });
        }).GeneratePdf(outputPath);
    }
}
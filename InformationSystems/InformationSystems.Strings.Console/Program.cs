using System;
using InformationSystems.Shared.Diagnostics;
using InformationSystems.Strings.Extensions;

string text = @"It was half-past five when we arrived at Yardly Chase, and followed the dignified butler to the old panelled hall
with its fire of blazing logs. A pretty picture met our eyes: Lady Yardly and her two children, the mother’s proud dark head bent
down over the two fair ones. Lord Yardly stood near, smiling down on them";

AssertIndex("was");
AssertIndex("them");
AssertIndex("Lord Yardly");

void AssertIndex(string entry)
{
    int hashIndex = text.HashIndexOf(entry);
    int index = text.IndexOf(entry, StringComparison.InvariantCulture);

    Assert.True(index == hashIndex);

    Console.WriteLine($"{entry} - index {hashIndex}");
}

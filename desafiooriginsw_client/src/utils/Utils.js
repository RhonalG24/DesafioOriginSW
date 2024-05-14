export function bankCardFormat(cardNumber) {
    let strArranged = [];
    for (let i = 0; i < cardNumber.length; i++) {
        if (i % 4 === 0 && i !== 0) {
            strArranged.push("-");
        }
        strArranged.push(cardNumber[i]);
    }
    let showingNumber = strArranged.join("");
    return showingNumber;
}

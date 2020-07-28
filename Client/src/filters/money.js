import numeral from "numeral"

export default function (value) {
    if (!value) {
        return 0;
      }

    return numeral(value).format("0,0");
}
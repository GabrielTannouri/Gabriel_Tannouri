$(function () {
    $(document).on("input", ".numeric", function (e) {
        this.value = this.value.replace(/[^0-9]/g, '');
    });

    $(".alert").fadeTo(5000, 500).slideUp(500, function () {
        $(".alert").slideUp(500);
    });

    jQuery(':input').keyup(function () {
        $(this).val($(this).val().toUpperCase());
    });

    jQuery('.text').keyup(function () {
        this.value = this.value.replace(/[^a-zA-Z\À-ú ]/g, '');
    });

    jQuery('.textNotSpace').keyup(function () {
        this.value = this.value.replace(/[^a-zA-Z]/g, '');
    });

    jQuery('.textDDI').keyup(function () {
        this.value = this.value.replace(/[^0-9+]/g, '');
    });

    $('.currency').each(function () {

        var aSep = $(this).attr('asep');
        var aDec = $(this).attr('adec');
        var vMin = $(this).attr('vmin');
        var vMax = $(this).attr('vmax');
        var mDec = $(this).attr('mdec');
        var metod = $(this).attr('metod');

        vMin = IsNullOrEmpty(vMin) ? '0' : vMin;
        vMax = IsNullOrEmpty(vMax) ? '9999999999.99999' : vMax;
        aSep = aSep == null ? '.' : aSep;
        aDec = IsNullOrEmpty(aDec) ? ',' : aDec;
        mDec = IsNullOrEmpty(mDec) ? '2' : mDec;
        metod = IsNullOrEmpty(metod) ? "init" : metod;

        $(this).autoNumeric(metod, {
            aSep: aSep,
            aDec: aDec,
            vMin: vMin,
            vMax: vMax,
            mDec: mDec,
        }).css('text-align', 'right');
    });

    $(".taxa").autoNumeric('init', {
        aSep: '.',
        aDec: ',',
        nBracket: '(,)',
        vMin: -999.99,
        vMax: 999.99
    }).css('text-align', 'right');

    $(".peso").autoNumeric('init', {
        aSep: '.',
        aDec: ',',
        nBracket: '(,)',
        vMin: 0,
        mDec: 3,
        vMax: 199.99
    }).css('text-align', 'right');

    $(".integer").autoNumeric('init', {
        vMin: 0,
        vMax: 9999999,
        mDec: 0,
        aSep: ''
    }).css('text-align', 'right');

});
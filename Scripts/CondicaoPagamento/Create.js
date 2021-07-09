$(function () {

    var con = new CondicaoPagamento();
    con.init();

    $('#addParcela').click(function () {
        con.adicionarParcela();
        return false;
    });

    $('#removeParcelas').click(function (e) {
        con.removeParcelas();
        return false;
    });

    function CondicaoPagamento() {
        const self = this;


        this.init = function () {
            self.dtParcela = new tDataTable({
                table: {
                    jsItem: "jsList",
                    name: "tblCondicao",
                    remove: false,
                    edit: false,
                    pageLength: 10,
                    "order": [[0, 'asc'], [1, 'asc']],
                    paginate: true,
                    columns: [
                        { data: "nrParcela" },
                        { data: "nmFormaPagamento" },
                        { data: "nrPrazo" },
                        { data: "nrPorcentagem" },
                    ]
                }
            });
            this.calcPorcentagem();
        }

        this.SavParcela = function () {
            this.adicionarParcela();
        };

        this.removeParcelas = function () {
            self.Limpar();
            self.dtParcela.data = null;
            self.calcPorcentagem();
        };

        this.adicionarParcela = function () {
            let nrParcela = 1;

            if (self.dtParcela.data != null && self.dtParcela.data.length > 0) {
                nrParcela = self.dtParcela.data.length + 1;
            }

            if (self.validarParcela()) {
                let item = {
                    nrParcela: nrParcela,
                    nmFormaPagamento: $("#formaPagamento_text").val(),
                    nrPrazo: $("#nrPrazo").val(),
                    nrPorcentagem: $("#nrPorcentagem").val(),
                    idFormaPagamento: $("#formaPagamento_idFormaPagamento").val(),
                };
                self.dtParcela.addItem(item);
                self.calcPorcentagem();
                self.Limpar();
            }

        };

        this.calcPorcentagem = function () {
            let nrPorcentagem = 0;
            self.dtParcela.data.forEach(function (data) {
                nrPorcentagem = nrPorcentagem + Functions.parseFloat(data.nrPorcentagem);
            });
            $("#nrTotalPorcentagem").val(nrPorcentagem.toFixed(2));
        };

        this.verificaPorcentagem = function (num) {
            let nrPorcentagem = Functions.parseFloat(num);
            self.dtParcela.data.forEach(function (data) {
                nrPorcentagem = nrPorcentagem + Functions.parseFloat(data.nrPorcentagem);
            });

            return nrPorcentagem.toFixed(2);
        };

        this.verificaDias = function (num) {
            let nrDias = 0;
            self.dtParcela.data.forEach(function (data) {
                if (data.nrPrazo > nrDias) {
                    nrDias = data.nrPrazo;
                }
            });
            return nrDias;
        };


        this.Limpar = function () {
            $("#nrPrazo").val("");
            $("#nrPorcentagem").val("");
            $("#formaPagamento_idFormaPagamento").val("");
            $("#formaPagamento_text").val("");
        };

        this.validarParcela = function () {
            let valid = true;

            if (this.verificaPorcentagem($("#nrPorcentagem").val()) > 100) {
                $.notify({ message: 'A porcentagem máxima é de 100%!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                valid = false;
            }
            if (this.verificaPorcentagem($("#nrPorcentagem").val()) <= 100) {

                if (IsNullOrEmpty($("#formaPagamento_text").val())) {
                    $.notify({ message: 'Por favor, informe a forma de pagamento!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                    valid = false;
                }

                if (IsNullOrEmpty($("#nrPrazo").val())) {
                    $.notify({ message: 'Por favor, informe o prazo!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                    valid = false;
                }

                if ($("#nrPrazo").val() < self.verificaDias()) {
                    $.notify({ message: 'Por favor, informe um prazo superior ao último informado!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                    valid = false;
                }

                if (IsNullOrEmpty($("#nrPorcentagem").val())) {
                    $.notify({ message: 'Por favor, informe a porcentagem!', icon: 'fa fa-exclamation' }, { type: 'danger' });
                    valid = false;
                }
            }

            return valid;
        };
    }
});

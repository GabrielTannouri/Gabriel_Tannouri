using Pratica_Profissional.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pratica_Profissional.DAO
{
    public class DAOCondicaoPagamento : DAO
    {

        public bool Create(CondicaoPagamento condicaoPagamento)
        {
            int i = 0;
            try
            {
                AbrirConexao();
                SqlTransaction sqlTrans = con.BeginTransaction();
                SqlCommand command = con.CreateCommand();
                command.Transaction = sqlTrans;

                command.CommandText = "INSERT INTO tbCondicaoPagamentos (nmcondicaopagamento, txjuros, multa, desconto, dtcadastro, dtatualizacao) VALUES " +
                    "(@nmcondicaopagamento, @txjuros, @multa, @desconto, @dtcadastro, @dtatualizacao);SELECT CAST(SCOPE_IDENTITY() AS int)";

                command.Parameters.AddWithValue("@nmcondicaopagamento", condicaoPagamento.nmCondicaoPagamento);
                command.Parameters.AddWithValue("@txjuros", condicaoPagamento.txJuros);
                command.Parameters.AddWithValue("@multa", condicaoPagamento.multa);
                command.Parameters.AddWithValue("@desconto", condicaoPagamento.desconto);
                command.Parameters.AddWithValue("@dtcadastro", condicaoPagamento.dtCadastro);
                command.Parameters.AddWithValue("@dtatualizacao", condicaoPagamento.dtAtualizacao);
                Int32 idRetorno = Convert.ToInt32(command.ExecuteScalar());

                command.CommandText = "INSERT INTO tbCondicaoPagamentoParcelas (idFormaPagamento, idCondicaoPagamento, nrParcela, nrPrazo, nrPorcentagem) VALUES " +
                    "(@idFormaPagamento, @idCondicaoPagamento, @nrParcela, @nrPrazo, @nrPorcentagem);";

                foreach (var item in condicaoPagamento.CondicaoParcelas)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@idFormaPagamento", item.idFormaPagamento);
                    command.Parameters.AddWithValue("@idCondicaoPagamento", idRetorno);
                    command.Parameters.AddWithValue("@nrParcela", item.nrParcela);
                    command.Parameters.AddWithValue("@nrPrazo", item.nrPrazo);
                    command.Parameters.AddWithValue("@nrPorcentagem", item.nrPorcentagem);
                    i = command.ExecuteNonQuery();
                }

                sqlTrans.Commit();

                // Tratamento para saber se alguma linha foi alterada no Banco de Dados

                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<CondicaoPagamento> GetCondicaoPagamentos()
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("SELECT * FROM tbCondicaoPagamentos", con);
                reader = SqlQuery.ExecuteReader();

                var lista = new List<CondicaoPagamento>();

                while (reader.Read())
                {
                    var condicaoPagamento = new CondicaoPagamento
                    {
                        idCondicaoPagamento = Convert.ToInt32(reader["idcondicaopagamento"]),
                        nmCondicaoPagamento = Convert.ToString(reader["nmcondicaopagamento"]),
                        txJuros = Convert.ToDecimal(reader["txjuros"]),
                        multa = Convert.ToDecimal(reader["multa"]),
                        desconto = Convert.ToDecimal(reader["desconto"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"]),
                    };

                    lista.Add(condicaoPagamento);
                }

                return lista;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            finally
            {
                FecharConexao();
            }
        }

        public CondicaoPagamento GetCondicaoPagamentosByID(int? idCondicaoPagamento)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idcondicaopagamento = " + idCondicaoPagamento;
                SqlQuery = new SqlCommand("SELECT * FROM tbCondicaoPagamentos" + _where, con);
                reader = SqlQuery.ExecuteReader();
                var objCondPagamento = new CondicaoPagamento();
                while (reader.Read())
                {
                    objCondPagamento = new CondicaoPagamento()
                    {
                        idCondicaoPagamento = Convert.ToInt32(reader["idcondicaopagamento"]),
                        nmCondicaoPagamento = Convert.ToString(reader["nmcondicaopagamento"]),
                        txJuros = Convert.ToDecimal(reader["txjuros"]),
                        multa = Convert.ToDecimal(reader["multa"]),
                        desconto = Convert.ToDecimal(reader["desconto"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"]),
                        CondicaoParcelas = this.GetParcelasByID(idCondicaoPagamento),
                    };
                }
                return objCondPagamento;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<CondicaoPagamentoParcela> GetParcelasByID(int? idCondicaoPagamento)
        {
            try
            {
                AbrirConexao();
                var _where = string.Empty;
                _where = " WHERE idcondicaopagamento = " + idCondicaoPagamento;
                SqlQuery = new SqlCommand("SELECT * FROM tbCondicaoPagamentoParcelas INNER JOIN tbFormaPagamentos on tbCondicaoPagamentoParcelas.idformapagamento = tbFormaPagamentos.idformapagamento" + _where, con);
                reader = SqlQuery.ExecuteReader();
                List<CondicaoPagamentoParcela> parcelas = new List<CondicaoPagamentoParcela>();
                while (reader.Read())
                {
                    var obj = new CondicaoPagamentoParcela()
                    {
                        idParcela = Convert.ToInt32(reader["idparcela"]),
                        idFormaPagamento = Convert.ToInt32(reader["idformapagamento"]),
                        nmFormaPagamento = Convert.ToString(reader["nmformapagamento"]),
                        idCondicaoPagamento = Convert.ToInt32(reader["idcondicaopagamento"]),
                        nrParcela = Convert.ToInt32(reader["nrparcela"]),
                        nrPrazo = Convert.ToInt32(reader["nrprazo"]),
                        nrPorcentagem = Convert.ToDecimal(reader["nrporcentagem"]),
                    };
                    parcelas.Add(obj);
                }

                return parcelas;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
  
        }

        public bool Edit(CondicaoPagamento condicaoPagamento)
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("UPDATE tbCondicaoPagamentos SET nmcondicaopagamento=@nmcondicaopagamento, txjuros=@txjuros, multa=@multa, desconto=@desconto, " +
                    "dtatualizacao=@dtAtualizacao WHERE idcondicaopagamento=@idcondicaopagamento", con);

                SqlQuery.Parameters.AddWithValue("@idcondicaopagamento", condicaoPagamento.idCondicaoPagamento);
                SqlQuery.Parameters.AddWithValue("@nmcondicaopagamento", condicaoPagamento.nmCondicaoPagamento);
                SqlQuery.Parameters.AddWithValue("@txjuros", condicaoPagamento.txJuros);
                SqlQuery.Parameters.AddWithValue("@multa", condicaoPagamento.multa);
                SqlQuery.Parameters.AddWithValue("@desconto", condicaoPagamento.desconto);
                SqlQuery.Parameters.AddWithValue("@dtatualizacao", condicaoPagamento.dtAtualizacao);

                // Validação para saber se a linha foi alterada no BD
                int i = SqlQuery.ExecuteNonQuery();
                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            finally
            {
                FecharConexao();
            }
        }

        public bool Delete(int id)
        {
            try
            {
                AbrirConexao();
                SqlQuery = new SqlCommand("DELETE FROM tbCondicaoPagamentos WHERE idcondicaopagamento=@idcondicaopagamento", con);

                SqlQuery.Parameters.AddWithValue("@idcondicaopagamento", id);
                // Validação para saber se a linha foi alterada no BD
                int i = SqlQuery.ExecuteNonQuery();
                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            finally
            {
                FecharConexao();
            }
        }
    }
}

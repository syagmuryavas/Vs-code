import pytest
from kitap import Kitap


def test_kitap_olusturuldugunda_musait_olmali():
    kitap = Kitap("Suç ve Ceza", "Dostoyevski")

    assert kitap.ad == "Suç ve Ceza"
    assert kitap.yazar == "Dostoyevski"
    assert kitap.durum == "musait"
    assert kitap.musait_mi() is True


def test_kitap_odunc_verildiginde_durum_oduncte_olmali():
    kitap = Kitap("1984", "George Orwell")

    kitap.odunc_ver()

    assert kitap.durum == "oduncte"
    assert kitap.musait_mi() is False


def test_oduncteki_kitap_tekrar_odunc_verilemez():
    kitap = Kitap("Dune", "Frank Herbert")

    kitap.odunc_ver()

    with pytest.raises(ValueError):
        kitap.odunc_ver()


def test_kitap_iade_edildiginde_tekrar_musait_olmali():
    kitap = Kitap("Sefiller", "Victor Hugo")

    kitap.odunc_ver()
    kitap.iade_al()

    assert kitap.durum == "musait"
    assert kitap.musait_mi() is True
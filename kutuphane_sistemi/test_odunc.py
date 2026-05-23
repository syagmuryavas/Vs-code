import pytest
from veritabani import Veritabani
from kutuphane import Kutuphane
from odunc import OduncYonetimi


def test_kitap_odunc_alinabilmeli():
    veritabani = Veritabani()
    kutuphane = Kutuphane(veritabani)
    odunc = OduncYonetimi(kutuphane)

    kutuphane.kitap_ekle("1984", "George Orwell")

    odunc.kitap_odunc_al("1984")

    assert odunc.kitap_musait_mi("1984") is False


def test_oduncteki_kitap_tekrar_odunc_alinamaz():
    veritabani = Veritabani()
    kutuphane = Kutuphane(veritabani)
    odunc = OduncYonetimi(kutuphane)

    kutuphane.kitap_ekle("Dune", "Frank Herbert")

    odunc.kitap_odunc_al("Dune")

    with pytest.raises(ValueError):
        odunc.kitap_odunc_al("Dune")


def test_kitap_iade_edilebilmeli():
    veritabani = Veritabani()
    kutuphane = Kutuphane(veritabani)
    odunc = OduncYonetimi(kutuphane)

    kutuphane.kitap_ekle("Sefiller", "Victor Hugo")

    odunc.kitap_odunc_al("Sefiller")
    odunc.kitap_iade_et("Sefiller")

    assert odunc.kitap_musait_mi("Sefiller") is True


def test_var_olmayan_kitap_odunc_alinamaz():
    veritabani = Veritabani()
    kutuphane = Kutuphane(veritabani)
    odunc = OduncYonetimi(kutuphane)

    with pytest.raises(ValueError):
        odunc.kitap_odunc_al("Bilinmeyen Kitap")
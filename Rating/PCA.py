import numpy as np
from sklearn.decomposition import PCA


class PCA_builder():
    def __init__(self):
        self.data = None

    def data_processing(self, data_trad_stat, data_adv_stat):
        data_trad_stat = np.array(data_trad_stat)
        data_adv_stat = np.array(data_adv_stat)
        return np.concatenate([data_trad_stat, data_adv_stat], axis=1)

    def make_PCA(self, data_trad_stat: list, data_adv_stat: list):
        length = len(data_trad_stat)
        self.data = self.data_processing(data_trad_stat, data_adv_stat)
        self.data.reshape((length, 2, -1)).mean(axis=1)
        pca = PCA(n_components=1)
        transformed_data = pca.fit_transform(self.data)
        return transformed_data

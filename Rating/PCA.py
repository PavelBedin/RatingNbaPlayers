import matplotlib.pyplot as plt
import numpy as np
from sklearn.decomposition import PCA


class PCA_builder():
    def __init__(self):
        self.data = None
        self.data_x = None
        self.data_y = None

    def data_processing(self, data_trad_stat, data_adv_stat):
        data_trad_stat = np.array(data_trad_stat)
        data_adv_stat = np.array(data_adv_stat)
        return np.concatenate([data_trad_stat, data_adv_stat], axis=1)

    def normalization(self, data):
        data_nor = np.array(data)
        std_data = np.std(data_nor)
        mean_val = np.mean(data_nor)
        return (data_nor - mean_val) / std_data

    def make_PCA(self, data_trad_stat: list, data_adv_stat: list):
        length = len(data_trad_stat)
        self.data = self.data_processing(data_trad_stat, data_adv_stat)
        self.data.reshape((length, 2, -1)).mean(axis=1)
        pca = PCA(n_components=2)
        transformed_data = pca.fit_transform(self.data)
        self.data_x = self.normalization([row[0] for row in transformed_data])
        self.data_y = self.normalization([row[1] for row in transformed_data])
        return list(zip(self.data_x, self.data_y))

    def show_new_component(self, data: list):
        x = [row[0] for row in data]
        y = [row[1] for row in data]
        plt.scatter(x, y, color='blue', marker='o')
        count = 1
        for i, txt in enumerate(y):
            plt.text(x[i], y[i], f'{count}', ha='left')
            count += 1

        plt.show()

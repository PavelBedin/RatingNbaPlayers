import matplotlib.pyplot as plt
from sklearn.decomposition import PCA


class PCA_builder():
    def __init__(self):
        pass

    def take_first_column(self, data):
        return [row[0] for row in data]

    def make_PCA(self, data_offensive: list, data_defensive: list):
        pca = PCA()
        transformed_offensive = pca.fit_transform(data_offensive)
        transformed_defensive = pca.fit_transform(data_defensive)
        return list(zip(self.take_first_column(transformed_offensive), self.take_first_column(transformed_defensive)))

    def show_new_component(self, x: list, y: list):
        plt.scatter(x, y, color='blue', marker='o')
        count = 1
        for i, txt in enumerate(y):
            plt.text(x[i], y[i], f'{count}', ha='left')
            count += 1

        plt.show()
